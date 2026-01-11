using System;
using MusicPlayer.Player;
using MusicPlayer.Utilities;
using Painter.Drawables.DrawableImpl;
using Painter.Drawing;

namespace MusicPlayer.Drawables;

public class PlayerProgressBar : Drawable
{
    private readonly IAudioPlayer _player;
    public PlayerProgressBar(IAudioPlayer player)
    {
        _player = player;
        _player.PlaybackTimeChangedEvent += RequestRedraw;
    }
    
    protected override void Draw(ICanvas canvas)
    {
        var width = Width;

        canvas.Draw(X, Y, $"[{_player.TrackTime:mm':'ss}]", new Style());
        canvas.Draw(X + width - 7, Y, $"[{_player.TrackLength:mm':'ss}]", new Style());

        width -= 16;
        var trackProgress = (int) Math.Ceiling(width * (_player.TrackTime / _player.TrackLength));
        var hueStep = 359d / width;
        for (var x = 0; x < trackProgress; x++)
        {
            canvas.Draw(X + x + 8, Y, " ", new Style
            {
                BackgroundColor = ColorHelper.FromHsv(hueStep * x, 1, 1)
            });
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