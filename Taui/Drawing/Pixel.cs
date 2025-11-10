namespace Taui.Drawing;

public record Pixel(char Character)
{
    public ConsoleColor ForegroundColor { get; init; } = ConsoleColor.White;
    
    public ConsoleColor BackgroundColor { get; init; } = ConsoleColor.Black;
    
}