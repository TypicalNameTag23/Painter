using Painter.Drawing;

namespace Painter.Components;

/// <summary>
/// Represents an object that can be drawn onto a <see cref="Canvas"/>.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Draw this object onto the provided <see cref="Canvas"/>.
    /// </summary>
    /// <param name="canvas">The <see cref="Canvas"/> to draw this object onto.</param>
    public void DrawOnto(Canvas canvas);
}