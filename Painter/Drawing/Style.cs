using System.Drawing;

namespace Painter.Drawing;

public record Style
{
    public Color? ForegroundColor { get; set; }
    public Color? BackgroundColor { get; set; }
    // public bool Bold { get; set; }
    // public bool Italic { get; set; }
    // public bool Underline { get; set; }
    // public bool DoubleUnderline { get; init; } // ?
    // public bool Strikethrough { get; set; }
}