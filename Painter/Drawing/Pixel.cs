namespace Taui.Drawing;

public record Pixel(char Character)
{
    public ConsoleColor? ForegroundColor { get; init; }
    
    public ConsoleColor? BackgroundColor { get; init; }
    
}