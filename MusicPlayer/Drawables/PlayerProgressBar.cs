using MusicPlayer.Player;
using Painter.Drawables;
using Painter.Drawing;
using Painter.Drawing.Canvas;

namespace MusicPlayer.Drawables;

public class PlayerProgressBar : Drawable
{
    private readonly IAudioPlayer _player;
    public PlayerProgressBar(IAudioPlayer player)
    {
        _player = player;
        _player.PlaybackTimeChangedEvent += RequestRedraw;
    }
    
    public override void Draw(ICanvas canvas)
    {
        var width = Width;

        canvas.Draw(X, Y, $"[{_player.TrackTime:mm':'ss}]", new StrokeStyle());
        canvas.Draw(X + width - 7, Y, $"[{_player.TrackLength:mm':'ss}]", new StrokeStyle());

        width -= 16;
        var trackProgress = (int) Math.Ceiling(width * (_player.TrackTime / _player.TrackLength));
        var hueStep = 359d / width;
        for (var x = 0; x < trackProgress; x++)
        {
            canvas.Draw(X + x + 8, Y, " ", new StrokeStyle
            {
                BackgroundColor = Color.FromHsv(hueStep * x, 1, 1)
            });
        }

        var brightnessStep = 0.2d / width;
        for (var x = trackProgress; x < width; x++)
        {
            canvas.Draw(X + x + 8, Y, " ", new StrokeStyle
            {
                BackgroundColor = Color.FromHsv(0, 0, 0.15 + brightnessStep * x)
            });
        }
    }
}