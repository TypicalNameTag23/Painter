using System.Drawing;
using MusicPlayer.Player;
using Painter.Drawing;
using Painter.Elements;

namespace MusicPlayer.Drawables;

public class PlaybackInformation : Element
{
    private readonly IAudioPlayer _player;
    private readonly TextBlock _textBlock;

    public PlaybackInformation(IAudioPlayer player)
    {
        _player = player;
        _textBlock = new TextBlock
        {
            Parent = this,
            Text = DisplayText(),
            Height = 1, // jank, please fix
            TextColor = Color.Aquamarine,
            Y = 3
        };

        _player.LoopTypeChangedEvent += () => _textBlock.Text = DisplayText();
        _player.PlayerStateChangedEvent += () => _textBlock.Text = DisplayText();
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        _textBlock.Draw(canvas, force);
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