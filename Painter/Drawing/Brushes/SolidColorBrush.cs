using System.Drawing;

namespace Painter.Drawing.Brushes;

public readonly struct SolidColorBrush(Color? color) : IBrush
{
    public Color? GetColorAt(int x, int y)
    {
        return color;
    }

    public static readonly SolidColorBrush TransparentColorBrush = new(Color.Transparent);
}