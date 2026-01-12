namespace Painter.Drawing;

public record StrokeStyle
{
    public Color? ForegroundColor { get; init; }
    public Color? BackgroundColor { get; init; }
    public bool Bold { get; init; }
    public bool Italic { get; init; }
    public bool Underline { get; init; }
    public bool Strikethrough { get; init; }
    public bool Blinking { get; init; }
}