using System.Drawing;
using MusicPlayer.Player;
using Painter.Drawing;
using Painter.Elements;

namespace MusicPlayer.Drawables;

public class PlayerTracksViewer : Element
{
    private static readonly Color SecondaryRowColor = Color.FromArgb(39, 39, 39);
    private static readonly Color SelectedRowColor = Color.FromArgb(186, 0, 91);
    private readonly List<TextBlock> _rows = [];
    private readonly IAudioPlayer _player;

    public PlayerTracksViewer(IAudioPlayer player)
    {
        _player = player;
        player.TrackChangedEvent += Redraw;
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        for (var i = 0; i < _player.Tracks.Count; i++)
        {
            if (_rows.Count <= i)
            {
                _rows.Add(new TextBlock
                {
                    Parent = this,
                    Height = 1,
                    Y = Y + i,
                    OverflowStyle = OverflowStyle.Ellipses,
                    TextColor = Color.Wheat
                });
            }

            var row = _rows[i];
            var trackMeta = _player.Tracks[i].TrackMeta;
            if (_player.CurrentTrackIndex == i)
            {
                row.BackgroundColor = SelectedRowColor;
            }
            else
            {
                row.BackgroundColor = i % 2 == 0 ? 
                    null : SecondaryRowColor;
            }
            
            row.Text = $"{trackMeta.Title} - {trackMeta.Artist} ({trackMeta.Album})";
            row.Draw(canvas, force);
        }

        if (_rows.Count <= _player.Tracks.Count)
        {
            return;
        }
        
        for (var i = _player.Tracks.Count; i < _rows.Count; i++)
        {
            _rows[i].Parent = null;
            _rows.RemoveAt(i);
        }
    }
}