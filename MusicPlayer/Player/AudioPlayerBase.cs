using MusicPlayer.Events;
using MusicPlayer.Models;

namespace MusicPlayer.Player;

public abstract class AudioPlayerBase : IAudioPlayer
{
    protected const int DefaultSeekDelayMilliseconds = 75;
    protected readonly List<AudioTrack> _tracks = [];
    protected int _seekDelayMilliseconds = DefaultSeekDelayMilliseconds;
    protected Timer? _playbackTimeUpdater;
    protected Timer? _seekDelayTimer;

    public virtual event PlaybackTimeChangedEvent? PlaybackTimeChangedEvent;
    public virtual event SeekTimeChangedEvent? SeekTimeChangedEvent;
    public virtual event TrackChangedEvent? TrackChangedEvent;
    public virtual event LoopTypeChangedEvent? LoopTypeChangedEvent;
    public virtual event PlayerStateChangedEvent? PlayerStateChangedEvent;

    public virtual int CurrentTrackIndex
    {
        get;
        set
        {
            Stop();
            field = value;
            TrackChangedEvent?.Invoke();
        }
    }

    public virtual PlayerState PlayerState
    {
        get;
        set
        {
            field = value;
            PlayerStateChangedEvent?.Invoke();
            if (field == PlayerState.Playing && _playbackTimeUpdater is null)
            {
                _playbackTimeUpdater = new Timer(_ =>
                {
                    PlaybackTimeChangedEvent?.Invoke();
                }, null, 0, 1000);
            }
            else
            {
                _playbackTimeUpdater?.DisposeAsync();
                _playbackTimeUpdater = null;
            }
        }
    } = PlayerState.Stopped;

    public virtual LoopType LoopType
    {
        get;
        set
        {
            field = value;
            LoopTypeChangedEvent?.Invoke();
        }
    } = LoopType.Disabled;
    
    public virtual IReadOnlyList<AudioTrack> Tracks => _tracks.AsReadOnly();
    public virtual AudioTrack? CurrentTrack => _tracks.Count == 0 ? null : Tracks[CurrentTrackIndex];
    public virtual TimeSpan TrackLength => CurrentTrack is null ? TimeSpan.Zero : CurrentTrack.TrackMeta.Duration;
    public abstract TimeSpan TrackPlayTime { get; }
    public virtual TimeSpan? SeekTime { get; set; }
    
    public abstract void Play();
    
    public abstract void Pause();
    
    public abstract void Stop();
    
    public abstract void SeekTo(TimeSpan time);
    
    public virtual void Queue(AudioTrack track)
    {
        _tracks.Add(track);
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
        _tracks.Insert(0, track);
    }

    public virtual void Dequeue(int trackIndex)
    {
        if (trackIndex >= _tracks.Count)
        {
            return;
        }

        _tracks.RemoveAt(trackIndex);
    }

    public virtual void Dequeue(AudioTrack track)
    {
        var trackIndex = _tracks.IndexOf(track);
        if (trackIndex == -1)
        {
            return;
        }
        
        Dequeue(trackIndex);
    }

    public virtual void ClearQueue()
    {
        _tracks.Clear();
    }
    
    public virtual void Seek(TimeSpan time)
    {
        if (PlayerState == PlayerState.Stopped)
        {
            return;
        }
        
        if (SeekTime is not null)
        {
            _seekDelayMilliseconds = DefaultSeekDelayMilliseconds;
            if (SeekTime.Value == TrackLength || SeekTime.Value == TimeSpan.Zero)
            {
                return;
            }
            
            AddSeekTime(time);
            return;
        }

        Pause();
        AddSeekTime(time);
        _seekDelayTimer = new Timer(_ =>
        {
            if (SeekTime is null)
            {
                return;
            }
            
            if (_seekDelayMilliseconds > 0)
            {
                _seekDelayMilliseconds -= 25;
                return;
            }

            var seekTime = SeekTime.Value;
            ResetSeekTimer();
            SeekTo(seekTime);
        }, null, 25, 25);
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
        Play();
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
        Play();
    }
    
    public virtual void Skip()
    {
        ResetSeekTimer();
        switch (LoopType)
        {
            case LoopType.LoopQueue:
            {
                if (CurrentTrackIndex == _tracks.Count - 1)
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
                if (CurrentTrackIndex == _tracks.Count - 1)
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

        Play();
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
                    CurrentTrackIndex--;
                    break;
                }
                
                Stop();
                break;
            }
            case LoopType.LoopTrack:
            {
                Stop();
                break;   
            }
        }

        Play();
    }

    public virtual void Dispose()
    {
        _playbackTimeUpdater?.Dispose();
        _seekDelayTimer?.Dispose();
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (_playbackTimeUpdater is not null)
        {
            await _playbackTimeUpdater.DisposeAsync();
        }
        
        if (_seekDelayTimer is not null)
        {
            await _seekDelayTimer.DisposeAsync();
        }
    }

    protected void OnPlaybackTimeChanged()
    {
        PlaybackTimeChangedEvent?.Invoke();
    }
    
    protected void OnSeekTimeChanged()
    {
        SeekTimeChangedEvent?.Invoke();
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
                if (CurrentTrackIndex == _tracks.Count - 1)
                {
                    CurrentTrackIndex = 0;
                    break;
                }

                CurrentTrackIndex++;
                break;
            }
            case LoopType.Disabled:
            {
                if (CurrentTrackIndex == _tracks.Count - 1)
                {
                    CurrentTrackIndex = 0;
                    return;
                }

                CurrentTrackIndex++;
                break;
            }
        }

        Stop();
        Play();
    }

    protected void ResetSeekTimer()
    {
        _seekDelayTimer?.Change(-1, -1);
        _seekDelayTimer?.Dispose();
        _seekDelayTimer = null;
        SeekTime = null;
        _seekDelayMilliseconds = DefaultSeekDelayMilliseconds;
    }
    
    private void AddSeekTime(TimeSpan time)
    {
        SeekTime ??= TrackPlayTime;
        SeekTime += time;
        if (SeekTime.Value >= TrackLength)
        {
            SeekTime = TrackLength;
        }
        else if (SeekTime.Value < TimeSpan.Zero)
        {
            SeekTime = TimeSpan.Zero;
        }
        
        SeekTimeChangedEvent?.Invoke();
    } 
}