using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicPlayer.Models;
using SoundFlow.Abstracts;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Interfaces;
using SoundFlow.Providers;
using SoundFlow.Structs;

namespace MusicPlayer.Player;

public sealed class SoundFlowAudioPlayer : AudioPlayerBase
{
    private readonly AudioEngine _engine;
    private readonly AudioFormat _format;
    private readonly AudioPlaybackDevice _device;
    private ISoundDataProvider? _audioStream;
    private SoundPlayer? _player;

    public SoundFlowAudioPlayer()
    {
        _engine = new MiniAudioEngine();
        _format = AudioFormat.StudioHq;
        
        var defaultPlaybackDevice = _engine.PlaybackDevices.FirstOrDefault(d => d.IsDefault);
        _device = _engine.InitializePlaybackDevice(defaultPlaybackDevice, _format);
        _device.Start();
        PlayerStateChangedEvent += () =>
        {
            if (PlayerState == PlayerState.Playing)
            {
                PlaybackTimeUpdater = new Timer(_ =>
                {
                    if (_player is null)
                    {
                        return;
                    }
                    
                    TrackTime = TimeSpan.FromSeconds(_player.Time);
                }, null, 250, 250);
            }
            else
            {
                PlaybackTimeUpdater?.Change(-1, -1);
                PlaybackTimeUpdater?.DisposeAsync();
                PlaybackTimeUpdater = null;
            }

            if (PlayerState == PlayerState.Stopped)
            {
                TrackTime = TimeSpan.Zero;
            }
        };
    }

    public override TimeSpan TrackTime 
    { 
        get;
        protected set
        {
            var time = value;
            if (time > TrackLength)
            {
                time = TrackLength;
            } 
            else if (time < TimeSpan.Zero)
            {
                time = TimeSpan.Zero;
            }
            
            if (time == field)
            {
                return;
            }

            field = time;
            OnPlaybackTimeChanged();
        } 
    }

    public override void Play(bool force = false)
    {
        switch (PlayerState)
        {
            case PlayerState.Playing:
            {
                if (!force)
                {
                    return;
                }
                
                break;
            }
            case PlayerState.Stopped:
            {
                ReadyTrack(TrackTime);
                break;
            }
        }

        if (_player is not null)
        {
            _player.Play();
            PlayerState = PlayerState.Playing;
        }
    }

    public override void Pause()
    {
        if (PlayerState == PlayerState.Stopped)
        {
            return;
        }
        
        _player?.Pause();
        PlayerState = PlayerState.Paused;
    }

    public override void Replay()
    {
        if (PlayerState == PlayerState.Stopped)
        {
            return;
        }
        
        ReadyTrack();
        if (_player is not null)
        {
            _player.Play();
            PlayerState = PlayerState.Playing;
        }
    }

    public override void Stop()
    {
        if (PlayerState == PlayerState.Stopped)
        {
            return;
        }
        
        ResetSeekTimer();
        if (_player is not null)
        {
            _player.Stop();
            _device.MasterMixer.RemoveComponent(_player);
            _player.Dispose();
            _player = null;
        }
        
        _audioStream?.Dispose();
        _audioStream = null;
        PlayerState = PlayerState.Stopped;
    }

    public override void Dispose()
    {
        _audioStream?.Dispose();
        _player?.Dispose();
        _device.Dispose();
        _engine.Dispose();
        
        base.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        _audioStream?.Dispose();
        _player?.Dispose();
        _device.Dispose();
        _engine.Dispose();
        
        await base.DisposeAsync();
    }

    protected override void StartSeekTimer()
    {
        var currentState = PlayerState;
        if (PlayerState == PlayerState.Playing)
        {
            Pause();
        }
        
        SeekDelayTimer = new Timer(_ =>
        {
            // Don't allow seeking while playing, must pause player
            if (PlayerState == PlayerState.Playing)
            {
                ResetSeekTimer();
                return;
            }
            
            if (SeekDelayMilliseconds > 0)
            {
                SeekDelayMilliseconds -= 25;
                return;
            }

            ResetSeekTimer();
            _player?.Seek(TrackTime);
            OnPlaybackTimeChanged();
            if (currentState == PlayerState.Playing)
            {
                Play();
            }
        }, null, 25, 25);
    }

    protected override void ReadyTrack(TimeSpan? startTime = null)
    {
        if (CurrentTrack is null)
        {
            return;
        }
                
        _audioStream?.Dispose();
        _audioStream = CurrentTrack.TrackSource switch
        {
            TrackSource.File => new StreamDataProvider(_engine, _format, File.OpenRead(CurrentTrack.Path)),
            TrackSource.Network => new NetworkDataProvider(_engine, _format, CurrentTrack.Path)
        };

        if (_player is not null)
        {
            _device.MasterMixer.RemoveComponent(_player);
            _player.Dispose();
        }
                
        _player = new SoundPlayer(_engine, _format, _audioStream);
        _player.PlaybackEnded += (_, _) => OnTrackEnd();
        _device.MasterMixer.AddComponent(_player);
        if (startTime is not null)
        {
            _player.Seek(startTime.Value);
            TrackTime = startTime.Value;
        }

        TrackTime = TimeSpan.Zero;
    }
}