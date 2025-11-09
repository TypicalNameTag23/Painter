using System.Drawing;
using Taui.Drawing;

namespace Taui.Utilities;

public class ConsoleUtils
{
    public static void Write(Pixel? pixel)
    {
        if (pixel is null)
        {
            Console.ResetColor();
            Console.Write('\0');
            return;
        }
        
        Console.ForegroundColor = pixel.ForegroundColor;
        Console.BackgroundColor = pixel.BackgroundColor;
        Console.Write($"{pixel.Character}");
    }
}