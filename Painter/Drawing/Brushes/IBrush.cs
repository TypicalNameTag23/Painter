using System.Drawing;

namespace Painter.Drawing.Brushes;

public interface IBrush
{
    public Color? GetColorAt(int x, int y);
}