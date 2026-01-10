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
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        var width = Width;

        canvas.Draw(X, Y, $"[{_player.TrackTime:mm':'ss}]", new Style());
        canvas.Draw(X + width - 7, Y, $"[{_player.TrackLength:mm':'ss}]", new Style());

        width -= 16;
        var trackProgress = (int) Math.Ceiling(width * (_player.TrackTime / _player.TrackLength));
        var hueStep = 359d / width;
        for (var x = 0; x < trackProgress; x++)
        {
            try
            {
                var hue = hueStep * x;
                if (hue > 359)
                {
                    Console.WriteLine("a");
                }
                
                canvas.Draw(X + x + 8, Y, " ", new Style
                {
                    BackgroundColor = ColorHelper.FromHsv(hue, 1, 1)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(trackProgress);
                Console.WriteLine(_player.CurrentTrackIndex);
                Console.WriteLine(width);
                Console.WriteLine($"{_player.TrackTime:mm':'ss}");
            }
        }

        var brightnessStep = 0.2d / width;
        for (var x = trackProgress; x < width; x++)
        {
            canvas.Draw(X + x + 8, Y, " ", new Style
            {
                BackgroundColor = ColorHelper.FromHsv(0, 0, 0.15 + brightnessStep * x)
            });
        }
    }
}