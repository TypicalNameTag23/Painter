using System.Drawing;
using MusicPlayer.Player;
using Painter.Drawables.DrawableImpl;
using Painter.Drawables.TextBlock;

namespace MusicPlayer.Drawables;

public class PlaybackInformation : Drawable
{
    private readonly IAudioPlayer _player;

    public PlaybackInformation(IAudioPlayer player)
    {
        _player = player;
        var textBlock = new TextBlock
        {
            Text = DisplayText(),
            Height = 1, // jank, please fix
            TextColor = Color.Aquamarine,
            Y = 3
        };
        
        Attach(textBlock);
        _player.LoopTypeChangedEvent += () => textBlock.Text = DisplayText();
        _player.PlayerStateChangedEvent += () => textBlock.Text = DisplayText();
    }

    private string DisplayText()
    {
        var playingString = _player.PlayerState == PlayerState.Playing ? "⏸" : "▶";
        var loopString = _player.LoopType switch
        {
            LoopType.LoopQueue => "↻",
            LoopType.LoopTrack => "⮔",
            _ => "⭢"
        };

        return $"[ {playingString} ] [ {loopString} ]";
    }
}