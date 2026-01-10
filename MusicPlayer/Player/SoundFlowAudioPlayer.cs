using MusicPlayer.Models;
using SoundFlow.Abstracts;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Enums;
using SoundFlow.Interfaces;
using SoundFlow.Providers;
using SoundFlow.Structs;

namespace MusicPlayer.Player;

public class SoundFlowAudioPlayer : AudioPlayerBase
{
    private readonly AudioEngine _engine;
    private readonly AudioFormat _format;
    private readonly AudioPlaybackDevice _device;
    private ISoundDataProvider? _audioStream;
    private SoundPlayer? _player;

    public SoundFlowAudioPlayer()
    {
        _engine = new MiniAudioEngine();
        _format = new()
        {
            Format = SampleFormat.F32,
            SampleRate = 48000,
            Channels = 2
        };
        
        var defaultPlaybackDevice = _engine.PlaybackDevices.FirstOrDefault(d => d.IsDefault);
        _device = _engine.InitializePlaybackDevice(defaultPlaybackDevice, _format);
        _device.Start();
    }

    public override TimeSpan TrackPlayTime => _player is not null ? 
        TimeSpan.FromSeconds(_player.Time) : TimeSpan.Zero;
    
    public override void Play()
    {
        switch (PlayerState)
        {
            case PlayerState.Playing:
            {
                return;
            }
            case PlayerState.Paused:
            {
                _player?.Play();
                PlayerState = PlayerState.Playing;
                break;
            }
            case PlayerState.Stopped:
            {
                if (CurrentTrack is null)
                {
                    break;
                }

                _audioStream = CurrentTrack.TrackSource switch
                {
                    TrackSource.File => new StreamDataProvider(_engine, _format, File.OpenRead(CurrentTrack.Path)),
                    TrackSource.Network => new NetworkDataProvider(_engine, _format, CurrentTrack.Path)
                };
                
                _player = new SoundPlayer(_engine, _format, _audioStream);
                _player.PlaybackEnded += (_, _) => OnTrackEnd();
                _device.MasterMixer.AddComponent(_player);
                _player.Play();
                
                PlayerState = PlayerState.Playing;
                break;
            }
        }
    }

    public override void Pause()
    {
        _player?.Pause();
        PlayerState = PlayerState.Paused;
    }
    
    public override void Stop()
    {
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
        OnPlaybackTimeChanged();
    }
    
    public override void SeekTo(TimeSpan time)
    {
        if (PlayerState == PlayerState.Stopped)
        {
            return;
        }

        ResetSeekTimer();
        Pause();
        if (time >= TrackLength)
        {
            OnTrackEnd();
            return;
        }

        if (time < TimeSpan.Zero)
        {
            time = TimeSpan.Zero;
        }

        _player?.Seek(time);
        OnPlaybackTimeChanged();
        Play();
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
}