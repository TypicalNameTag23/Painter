using System.Drawing;

namespace Painter.Drawing.Brushes;

public interface IBrush
{
    /// <summary>
    /// Gets the color described by this brush at the specified coordinates given the width and the height
    /// </summary>
    /// <param name="x">The x coordinate</param>
    /// <param name="y">The y coordinate</param>
    /// <param name="width">The width of the area that this brush is being used in</param>
    /// <param name="height">The height of the area that this brush is being used in</param>
    /// <returns></returns>
    public Color? GetColorAt(int x, int y, int width, int height);
}