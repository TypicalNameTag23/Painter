using System.Drawing;
using MusicPlayer.Player;
using Painter.Drawing;
using Painter.Elements;

namespace MusicPlayer.Drawables;

public class PlayerTrackInfo : Element
{
    private readonly TextBlock _textBlock;
    private readonly IAudioPlayer _player;

    public PlayerTrackInfo(IAudioPlayer player)
    {
        _player = player;
        _textBlock = new TextBlock
        {
            Parent = this,
            Height = 1,
            OverflowStyle = OverflowStyle.Ellipses,
            Text = GetDisplayTitle(),
            TextColor = Color.Aquamarine
        };
        
        player.TrackChangedEvent += () =>
        {
            _textBlock.Text = GetDisplayTitle();
        };
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        _textBlock.Draw(canvas, force);
    }

    private string GetDisplayTitle()
    {
        var track = _player.CurrentTrack;
        return track is null
            ? "[Nothing Playing]"
            : $"{track.TrackMeta.Title} - {track.TrackMeta.Artist} ({track.TrackMeta.Album})";
    }
}