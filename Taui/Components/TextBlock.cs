using System.Drawing;
using Taui.Drawing;

namespace Taui.Composeables;

public class TextBlock : IComposable
{
    public string Text { get; set; } = "";
    
    public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
    
    public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

    public int X { get; set; } = 0;

    public int Y { get; set; } = 0;

    public void ComposeOnto(Canvas canvas)
    {
        for (var i = 0; i < Text.Length; i++)
        {
            canvas.Draw(X + i, Y, Text[i], TextColor, BackgroundColor);
        }
    }

}