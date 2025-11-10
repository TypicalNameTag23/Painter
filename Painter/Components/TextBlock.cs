using Painter.Drawing;
using Point = Painter.Drawing.Point;

namespace Painter.Components;

/// <summary>
/// A basic <see cref="TextBlock"/> for displaying text to the UI.
/// </summary>
public class TextBlock : IDrawable // todo: text wrapping
{
    /// <summary>
    /// The text to display to the UI.
    /// </summary>
    public string Text { get; set; } = "";
    
    /// <summary>
    /// The color of the text being displayed.
    /// </summary>
    public ConsoleColor? TextColor { get; set; } = null;
    
    /// <summary>
    /// The background color of the text being displayed.
    /// </summary>
    public ConsoleColor? BackgroundColor { get; set; } = null; // inherit from parent background color

    /// <summary>
    /// The X position of this text on the <see cref="Canvas"/>.
    /// </summary>
    public int X { get; set; } = 0;

    /// <summary>
    /// The Y position of this text on the <see cref="Canvas"/>.
    /// </summary>
    public int Y { get; set; } = 0;

    public void DrawOnto(Canvas canvas)
    {
        for (var i = 0; i < Text.Length && i < canvas.Width; i++)
        {
            var point = new Point(X + i, Y);
            var pixel = new Pixel(Text[i])
            {
                ForegroundColor = TextColor,
                BackgroundColor = BackgroundColor
            };
            
            canvas.Draw(point, pixel);
        }
    }

}