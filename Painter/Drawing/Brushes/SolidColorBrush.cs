namespace Painter.Drawing.Brushes;

public record SolidColorBrush(Color? Color) : IBrush
{
    public Color? GetColorAt(int x, int y, int width, int height)
    {
        return Color;
    }

    public static readonly SolidColorBrush TransparentColorBrush = new(Drawing.Color.Transparent);
}