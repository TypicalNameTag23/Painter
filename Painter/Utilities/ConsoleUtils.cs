using Taui.Drawing;
using Point = Taui.Drawing.Point;

namespace Taui.Utilities;

public static class ConsoleUtils
{
    public static void WriteTo(Point point, Pixel? pixel)
    {
        Console.SetCursorPosition(point.X, point.Y);
        if (pixel is null)
        {
            Console.ResetColor();
            Console.Write(" ");
            return;
        }

        if (pixel.ForegroundColor is not null)
        {
            Console.ForegroundColor = (ConsoleColor) pixel.ForegroundColor;
        }
        
        if (pixel.BackgroundColor is not null)
        {
            Console.ForegroundColor = (ConsoleColor) pixel.BackgroundColor;
        }
        Console.Write(pixel.Character);
        Console.ResetColor();
    }
}