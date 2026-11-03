using MusicPlayer.Player;
using Painter.Drawables;
using Painter.Drawing;

namespace MusicPlayer.Drawables;

public class PlayerTrackInfo : Drawable
{
    private readonly IAudioPlayer _player;

    public PlayerTrackInfo(IAudioPlayer player)
    {
        _player = player;
        var textBlock = new TextBlock
        {
            Height = 1,
            OverflowStyle = OverflowStyle.Ellipses,
            Text = GetDisplayTitle(),
            TextColor = Color.DarkSeaGreen
        };
        
        Attach(textBlock);
        player.TrackChangedEvent += () =>
        {
            textBlock.Text = GetDisplayTitle();
        };
    }

    private string GetDisplayTitle()
    {
        var track = _player.CurrentTrack;
        return track is null
            ? "[Nothing Playing]"
            : $"{track.TrackMeta.Title} - {track.TrackMeta.Artist} ({track.TrackMeta.Album})";
    }
}