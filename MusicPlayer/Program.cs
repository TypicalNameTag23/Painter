using MusicPlayer.Drawables;
using MusicPlayer.Models;
using MusicPlayer.Player;
using Painter;
using Painter.Drawables;

namespace MusicPlayer;

class Program
{
    private const string AudioFolder = @"/home/typicalnametag23/Music/Celeste OST/Celeste Original Soundtrack";
    private static IAudioPlayer _player = new SoundFlowAudioPlayer();
    private static readonly TimeSpan SeekSmall = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan SeekMedium = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan SeekLarge = TimeSpan.FromMinutes(1);
    
    static async Task Main()
    {
        var tracks = new System.Collections.Generic.List<AudioTrack>();
        foreach (var filePath in Directory.GetFiles(AudioFolder))
        {
            var track = await AudioTrackFactory.CreateFromFileAsync(filePath);
            if (track is not null)
            {
                tracks.Add(track);
            }
        }
        
        tracks.Sort();
        _player.Queue(tracks);
        var application = new Application
        {
            Content = new StackPanel
            {
                Content = [
                    new PlayerTrackInfo(_player)
                    {
                        Height = 1
                    },
                    new PlayerProgressBar(_player)
                    {
                        Height = 1
                    },
                    new PlaybackInformation(_player)
                    {
                        Height = 1
                    },
                    new PlayerTracksViewer(_player)
                ]
            }
        };

        application.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), (_, _) => application.Kill());
        application.AddGlobalKeyBind(new Key(ConsoleKey.Spacebar), (_, _) => TogglePlay());
        application.AddGlobalKeyBind(new Key(ConsoleKey.L), (_, _) => SwitchLoopType());
        
        application.AddGlobalKeyBind(new Key(ConsoleKey.UpArrow), (_, _) => _player.SkipBack());
        application.AddGlobalKeyBind(new Key(ConsoleKey.DownArrow), (_, _) => _player.Skip());
        application.AddGlobalKeyBind(new Key(ConsoleKey.UpArrow, ConsoleModifiers.Control), (_, _) => _player.PreviousTrack());
        application.AddGlobalKeyBind(new Key(ConsoleKey.DownArrow, ConsoleModifiers.Control), (_, _) => _player.NextTrack());

        application.AddGlobalKeyBind(new Key(ConsoleKey.RightArrow),
            (_, _) => _player.Seek(SeekSmall));
        application.AddGlobalKeyBind(new Key(ConsoleKey.RightArrow, ConsoleModifiers.Shift),
            (_, _) => _player.Seek(SeekMedium));
        application.AddGlobalKeyBind(new Key(ConsoleKey.RightArrow, ConsoleModifiers.Control), 
            (_, _) => _player.Seek(SeekLarge));
        application.AddGlobalKeyBind(new Key(ConsoleKey.LeftArrow),
            (_, _) => _player.Seek(-SeekSmall));
        application.AddGlobalKeyBind(new Key(ConsoleKey.LeftArrow, ConsoleModifiers.Shift), 
            (_, _) => _player.Seek(-SeekMedium));
        application.AddGlobalKeyBind(new Key(ConsoleKey.LeftArrow, ConsoleModifiers.Control), 
            (_, _) => _player.Seek(-SeekLarge));
        
        application.AddGlobalKeyBind(new Key(ConsoleKey.D0), (_, _) => _player.SeekTo(TimeSpan.Zero));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D1), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(1)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D2), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(2)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D3), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(3)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D4), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(4)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D5), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(5)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D6), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(6)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D7), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(7)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D8), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(8)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.D9), (_, _) => _player.SeekTo(TimeSpan.FromMinutes(9)));
        application.AddGlobalKeyBind(new Key(ConsoleKey.Subtract), (_, _) => SeekToStart());
        application.Start();
    }

    private static void SeekToStart()
    {
        Task.Run(async () =>
        {
            const int duration = 5;
            const int iterations = 5000;
            
            _player.Pause();
            var step = _player.TrackTime.TotalSeconds / iterations;
            var delayPerIteration = (1000 * duration) / iterations;
            for (var i = 1; i <= iterations; i++)
            {
                _player.SeekTo(_player.TrackTime - TimeSpan.FromSeconds(step));
                await Task.Delay(delayPerIteration);
            }

            _player.Play();
        });
    }

    private static void SwitchLoopType()
    {
        _player.LoopType = _player.LoopType switch
        {
            LoopType.LoopQueue => LoopType.LoopTrack,
            LoopType.LoopTrack => LoopType.Disabled,
            _ => LoopType.LoopQueue
        };
    }

    private static void TogglePlay()
    {
        switch (_player.PlayerState)
        {
            case PlayerState.Playing:
            {
                _player.Pause();
                break;
            }
            case PlayerState.Paused:
            case PlayerState.Stopped:
            {
                _player.Play();
                break;
            }
        }
    }
}