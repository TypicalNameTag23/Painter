using MusicPlayer.Player;
using MusicPlayer.Utilities;
using Painter.Drawing;
using Painter.Elements;

namespace MusicPlayer.Drawables;

public class PlayerProgressBar : Element
{
    private readonly IAudioPlayer _player;
    public PlayerProgressBar(IAudioPlayer player)
    {
        _player = player;
        _player.PlaybackTimeChangedEvent += Redraw;
        _player.SeekTimeChangedEvent += Redraw;
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        var width = Width;

        var progressTime = _player.SeekTime ?? _player.TrackPlayTime;
        canvas.Draw(X, Y, $"[{progressTime:mm':'ss}]", new Style());
        canvas.Draw(X + width - 7, Y, $"[{_player.TrackLength:mm':'ss}]", new Style());

        width -= 16;
        var trackProgress = (int) Math.Ceiling(width * (progressTime / _player.TrackLength));
        var hueStep = 359d / width;
        for (var x = 0; x < trackProgress; x++)
        {
            try
            {
                canvas.Draw(X + x + 8, Y, " ", new Style
                {
                    BackgroundColor = ColorHelper.FromHsv(hueStep * x, 1, 1)
                });
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(progressTime);
                Console.WriteLine(_player.TrackLength);
                throw e;
            }
            
        }

        var brightnessStep = 0.8d / width;
        for (var x = trackProgress; x < width; x++)
        {
            canvas.Draw(X + x + 8, Y, " ", new Style
            {
                BackgroundColor = ColorHelper.FromHsv(285, 0.2 + brightnessStep * x, 0.5)
            });
        }
    }
}