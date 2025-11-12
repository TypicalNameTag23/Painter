using System.Drawing;
using Painter.Drawing;

namespace Painter.Components;

/// <summary>
/// A basic <see cref="TextBlock"/> for displaying text to the UI.
/// </summary>
public class TextBlock : Drawable // todo: text wrapping
{
    /// <summary>
    /// The text to display to the UI.
    /// </summary>
    public string Text { get; set; } = "";
    
    /// <summary>
    /// The color of the text being displayed.
    /// </summary>
    public Color? TextColor { get; set; }
    
    /// <summary>
    /// The background color of the text being displayed.
    /// </summary>
    public Color? BackgroundColor { get; set; } // inherit from parent background color

    /// <summary>
    /// The X position of this text on the <see cref="Canvas"/>.
    /// </summary>
    public int X { get; set; } = 0;

    /// <summary>
    /// The Y position of this text on the <see cref="Canvas"/>.
    /// </summary>
    public int Y { get; set; } = 0;

    public override void DrawOnto(Canvas canvas)
    {
        canvas.Draw(X, Y, Text, TextColor, BackgroundColor);
    }

}