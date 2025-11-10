using Painter.Drawing;
using Painter.Exceptions;

namespace Painter.Utilities;

public static class ConsoleUtils
{
    /// <summary>
    /// Write a <see cref="Pixel"/> to a <see cref="Point"/> on the console.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to print to.</param>
    /// <param name="pixel">The <see cref="Pixel"/> to print.</param>
    /// <exception cref="PainterException">Thrown when trying to print to a <see cref="Point"/> outside the terminal</exception>
    public static void WriteTo(Point point, Pixel? pixel)
    {
        if (point.X >= Application.Width || point.Y >= Application.Height || point.X < 0 || point.Y < 0)
        {
            throw new PainterException(
                $"Trying to print to point outside of the terminal (x: {point.X}, y: {point.Y}) (width: {Application.Width}, height: {Application.Height})");
        }
        
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
            Console.BackgroundColor = (ConsoleColor) pixel.BackgroundColor;
        }
        
        Console.Write(pixel.Character);
        Console.ResetColor();
    }
}