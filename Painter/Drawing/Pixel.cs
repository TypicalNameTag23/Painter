namespace Painter.Drawing;

public readonly struct Pixel(char? character, StrokeStyle? strokeStyle)
{
    public char? Character { get; } = character;
    public StrokeStyle? StrokeStyle { get; } = strokeStyle;
}