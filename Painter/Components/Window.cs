using Painter.Drawing;

namespace Painter.Components;

/// <summary>
/// Base <see cref="Window"/> class.
/// </summary>
public class Window : IDrawable
{
    /// <summary>
    /// The content of this <see cref="Window"/>.
    /// </summary>
    public IDrawable? Drawable { get; set; }

    /// <summary>
    /// Draw the content of this <see cref="Window"/> onto the provided <see cref="Canvas"/>.
    /// </summary>
    /// <param name="canvas">The <see cref="Canvas"/> to draw the content of this <see cref="Window"/> onto.</param>
    public void DrawOnto(Canvas canvas)
    {
        Drawable?.DrawOnto(canvas);
    }
}