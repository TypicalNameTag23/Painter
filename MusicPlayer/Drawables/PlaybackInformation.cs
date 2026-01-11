using System.Drawing;
using MusicPlayer.Player;
using Painter.Drawables;

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
            TextColor = Color.Aquamarine,
            Height = 1
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