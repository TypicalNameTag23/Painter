using MusicPlayer.Drawables;
using MusicPlayer.Models;
using MusicPlayer.Player;
using Painter;
using Painter.Layouts;

namespace MusicPlayer;

class Program
{
    private const string AudioFolder = @"/home/typicalnametag23/Music/Portugual. The Man/Evil Friends";
    private static IAudioPlayer _player = new SoundFlowAudioPlayer();
    private static readonly TimeSpan SeekSmall = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan SeekMedium = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan SeekLarge = TimeSpan.FromMinutes(1);
    
    static async Task Main()
    {
        var tracks = new List<AudioTrack>();
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
            Drawable = new Panel
            {
                Elements = [
                    new PlayerTrackInfo(_player),
                    new PlayerProgressBar(_player)
                    {
                        Y = 2
                    },
                    new PlaybackInformation(_player)
                    {
                        Y = 3
                    },
                    new PlayerTracksViewer(_player)
                    {
                        Y = 6
                    },
                    // new HsvColorSpaceVisualizer()
                ]
            }
        };

        application.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), application.Kill);
        application.AddGlobalKeyBind(new Key(ConsoleKey.Spacebar), TogglePlay);
        application.AddGlobalKeyBind(new Key(ConsoleKey.UpArrow), _player.SkipBack);
        application.AddGlobalKeyBind(new Key(ConsoleKey.DownArrow), _player.Skip);
        application.AddGlobalKeyBind(new Key(ConsoleKey.L), SwitchLoopType);
        application.AddGlobalKeyBind(new Key(ConsoleKey.RightArrow), 
            () => _player.Seek(SeekSmall));
        application.AddGlobalKeyBind(new Key(ConsoleKey.RightArrow, ConsoleModifiers.Shift), 
            () => _player.Seek(SeekMedium));
        application.AddGlobalKeyBind(new Key(ConsoleKey.RightArrow, ConsoleModifiers.Control), 
            () => _player.Seek(SeekLarge));
        application.AddGlobalKeyBind(new Key(ConsoleKey.LeftArrow), 
            () => _player.Seek(-SeekSmall));
        application.AddGlobalKeyBind(new Key(ConsoleKey.LeftArrow, ConsoleModifiers.Shift), 
            () => _player.Seek(-SeekMedium));
        application.AddGlobalKeyBind(new Key(ConsoleKey.LeftArrow, ConsoleModifiers.Control), 
            () => _player.Seek(-SeekLarge));

        application.Start();
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