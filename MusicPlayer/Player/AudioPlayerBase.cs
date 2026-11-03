using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicPlayer.Events;
using MusicPlayer.Models;

namespace MusicPlayer.Player;

public abstract class AudioPlayerBase : IAudioPlayer
{
    private const int DefaultInitialSeekDelayMilliseconds = 50;
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly List<AudioTrack> TrackList = [];
    protected int SeekDelayMilliseconds = DefaultInitialSeekDelayMilliseconds;
    protected Timer? PlaybackTimeUpdater;
    protected Timer? SeekDelayTimer;

    public event PlaybackTimeChangedEvent? PlaybackTimeChangedEvent;
    public event TrackChangedEvent? TrackChangedEvent;
    public event LoopTypeChangedEvent? LoopTypeChangedEvent;
    public event PlayerStateChangedEvent? PlayerStateChangedEvent;

    public virtual int CurrentTrackIndex
    {
        get;
        set
        {
            if (field == value)
            {
                return;
            }

            field = value;
            // Seek timer might be buffering inputs when trying to navigate tracks and will cause issues
            // Reset the seek timer to disable this
            ResetSeekTimer();
            TrackTime = TimeSpan.Zero;
            if (PlayerState != PlayerState.Stopped)
            {
                ReadyTrack();
                Play(force: true);
            }

            TrackChangedEvent?.Invoke();
        }
    }

    public virtual PlayerState PlayerState
    {
        get;
        set
        {
            if (field == value)
            {
                return;
            }
            
            field = value;
            PlayerStateChangedEvent?.Invoke();
        }
    } = PlayerState.Stopped;

    public virtual LoopType LoopType
    {
        get;
        set
        {
            if (field == value)
            {
                return;
            }
            
            field = value;
            LoopTypeChangedEvent?.Invoke();
        }
        
    } = LoopType.Disabled;
    
    public virtual IReadOnlyList<AudioTrack> Tracks => TrackList.AsReadOnly();
    public virtual AudioTrack? CurrentTrack => TrackList.Count == 0 ? null : Tracks[CurrentTrackIndex];
    public virtual TimeSpan TrackLength => CurrentTrack is null ? TimeSpan.Zero : CurrentTrack.TrackMeta.Duration;
    public abstract TimeSpan TrackTime { get; protected set; }

    public abstract void Play(bool force = false);
    
    public abstract void Pause();
    
    public abstract void Replay();
    
    public abstract void Stop();
    
    protected abstract void ReadyTrack(TimeSpan? startTime = null);
    
    public virtual void Queue(AudioTrack track)
    {
        TrackList.Add(track);
    }

    public virtual void Queue(List<AudioTrack> tracks)
    {
        foreach (var audioTrack in tracks)
        {
            Queue(audioTrack);
        }
    }

    public virtual void QueueNext(AudioTrack track)
    {
        TrackList.Insert(0, track);
    }

    public virtual void Dequeue(int trackIndex)
    {
        if (trackIndex >= TrackList.Count)
        {
            return;
        }

        TrackList.RemoveAt(trackIndex);
    }

    public virtual void Dequeue(AudioTrack track)
    {
        var trackIndex = TrackList.IndexOf(track);
        if (trackIndex == -1)
        {
            return;
        }
        
        Dequeue(trackIndex);
    }

    public virtual void ClearQueue()
    {
        TrackList.Clear();
    }
    
    public virtual void SeekTo(TimeSpan time)
    {
        SetSeekTime(time);
    }
    
    public virtual void Seek(TimeSpan time)
    {
        SeekTo(TrackTime + time);
    }

    public virtual void NextTrack()
    {        
        ResetSeekTimer();
        if (CurrentTrackIndex + 1 >= Tracks.Count)
        {
            Stop();
            return;
        }

        CurrentTrackIndex++;
    }

    public virtual void PreviousTrack()
    {
        ResetSeekTimer();
        if (CurrentTrackIndex <= 0)
        {
            Stop();
            return;
        }

        CurrentTrackIndex--;
    }
    
    public virtual void Skip()
    {
        ResetSeekTimer();
        PlayerState = PlayerState.Playing;
        switch (LoopType)
        {
            case LoopType.LoopQueue:
            {
                if (CurrentTrackIndex == TrackList.Count - 1)
                {
                    CurrentTrackIndex = 0;
                    break;
                }

                CurrentTrackIndex++;
                break;
            }
            case LoopType.LoopTrack:
            case LoopType.Disabled:
            {
                if (CurrentTrackIndex == TrackList.Count - 1)
                {
                    return;
                }

                if (LoopType == LoopType.LoopTrack)
                {
                    LoopType = LoopType.Disabled;
                }
                
                CurrentTrackIndex++;
                break;
            }
        }
    }

    public virtual void SkipBack()
    {
        ResetSeekTimer();
        switch (LoopType)
        {
            case LoopType.LoopQueue:
            case LoopType.Disabled:
            {
                if (CurrentTrackIndex > 0)
                {
                    PlayerState = PlayerState.Playing;
                    CurrentTrackIndex--;
                    return;
                }
                
                break;
            }
        }
        
        Replay();
    }

    public virtual void Dispose()
    {
        PlaybackTimeUpdater?.Dispose();
        SeekDelayTimer?.Dispose();
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (PlaybackTimeUpdater is not null)
        {
            await PlaybackTimeUpdater.DisposeAsync();
        }
        
        if (SeekDelayTimer is not null)
        {
            await SeekDelayTimer.DisposeAsync();
        }
    }

    protected void OnPlaybackTimeChanged()
    {
        PlaybackTimeChangedEvent?.Invoke();
    }
    
    protected void OnTrackChanged()
    {
        TrackChangedEvent?.Invoke();
    }
    
    protected void OnLoopTypeChanged()
    {
        LoopTypeChangedEvent?.Invoke();
    }
    
    protected void OnPlayerStateChanged()
    {
        PlayerStateChangedEvent?.Invoke();
    }

    protected void OnTrackEnd()
    {
        switch (LoopType)
        {
            case LoopType.LoopQueue:
            {
                if (CurrentTrackIndex == TrackList.Count - 1)
                {
                    CurrentTrackIndex = 0;
                    break;
                }

                CurrentTrackIndex++;
                break;
            }
            case LoopType.LoopTrack:
            {
                Replay();
                break;
            }
            case LoopType.Disabled:
            {
                if (CurrentTrackIndex == TrackList.Count - 1)
                {
                    Stop();
                    CurrentTrackIndex = 0;
                    return;
                }

                CurrentTrackIndex++;
                break;
            }
        }
    }

    protected void SetSeekTime(TimeSpan time)
    {
        TrackTime = time;
        SeekDelayMilliseconds = DefaultInitialSeekDelayMilliseconds;
        if (SeekDelayTimer is null)
        {
            StartSeekTimer();
        }
    }

    protected void ResetSeekTimer()
    {
        if (SeekDelayTimer is null)
        {
            return;
        }
        
        SeekDelayTimer.Change(-1, -1);
        SeekDelayTimer.Dispose();
        SeekDelayTimer = null;
        SeekDelayMilliseconds = DefaultInitialSeekDelayMilliseconds;
    }

    protected abstract void StartSeekTimer();
}