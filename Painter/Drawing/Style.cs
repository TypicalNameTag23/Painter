using System.Drawing;

namespace Painter.Drawing;

public record Style
{
    public Color? ForegroundColor { get; init; }
    public Color? BackgroundColor { get; init; }
    public bool Bold { get; init; }
    public bool Italic { get; init; }
    public bool Underline { get; init; }
    public bool Strikethrough { get; init; }
}