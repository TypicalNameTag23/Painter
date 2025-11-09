namespace Taui.Drawing;

public class Pixel(char character)
{
    public char Character { get; } = character;
    
    public ConsoleColor ForegroundColor { get; init; } = ConsoleColor.White;
    
    public ConsoleColor BackgroundColor { get; init; } = ConsoleColor.Black;
    
}