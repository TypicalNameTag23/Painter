using System.Drawing;
using Taui.Drawing;
using Point = Taui.Drawing.Point;

namespace Taui.Composeables;

public class TextBlock : IDrawable
{
    public string Text { get; set; } = "";
    
    public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
    
    public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

    public int X { get; set; } = 0;

    public int Y { get; set; } = 0;

    public void DrawOnto(Canvas canvas)
    {
        for (var i = 0; i < Text.Length; i++)
        {
            var point = new Point(X + i, Y);
            var pixel = new Pixel(Text[i])
            {
                ForegroundColor = TextColor,
                BackgroundColor = BackgroundColor
            };
            
            canvas.Draw(point, pixel);
        }
    }

}