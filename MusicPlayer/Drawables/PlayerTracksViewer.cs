using MusicPlayer.Player;
using Painter.Drawables;
using Painter.Drawing;

namespace MusicPlayer.Drawables;

// todo: kinda jank broken rn, will come back and make changes to improve this later
public class PlayerTracksViewer : Drawable
{
    private static readonly Color SecondaryRowColor = new(39, 39, 39);
    private static readonly Color SelectedRowColor = new(186, 0, 91);
    private readonly List<TextBlock> _rows = [];
    private readonly IAudioPlayer _player;

    public PlayerTracksViewer(IAudioPlayer player)
    {
        _player = player;
        UpdateRows();
        player.TrackChangedEvent += UpdateRows;
    }

    private void UpdateRows()
    {
        for (var i = 0; i < _player.Tracks.Count; i++)
        {
            if (_rows.Count <= i)
            {
                var textBlock = new TextBlock
                {
                    Height = 1,
                    Y = i,
                    OverflowStyle = OverflowStyle.Ellipses,
                    // TextColor = Color.Wheat
                };
                
                _rows.Add(textBlock);
                Attach(textBlock);
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
        }

        if (_rows.Count <= _player.Tracks.Count)
        {
            return;
        }
        
        for (var i = _player.Tracks.Count; i < _rows.Count; i++)
        {
            Detach(_rows[i]);
            _rows.RemoveAt(i);
        }
    }
}