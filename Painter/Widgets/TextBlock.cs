using System.Drawing;
using Painter.Drawing;

namespace Painter.Widgets;

public class TextBlock : Widget
{
    public string Text { get; set; } = "";
    
    public Color? TextColor { get; set; }
    
    public Color? BackgroundColor { get; set; }
    
    public bool WrapText { get; set; } = false;
    
    public OverflowStyle OverflowStyle { get; set; } = OverflowStyle.Cutoff;
    
    public override void Draw(Canvas canvas)
    {
        if (Width is null || Height is null)
        {
            return;
        }
        
        var lines = new string[(int) Height];
        if (WrapText)
        {
            lines[0] = Text[..(int) Width];
            var lineIndex = 1;
            var textIndex = Width;
            for (; textIndex < Text.Length && lineIndex < Height; textIndex += Width, lineIndex++)
            {
                lines[lineIndex] = Text.Substring((int) (textIndex), (int) (textIndex + Width));
            }
        }
        else
        {
            switch (OverflowStyle)
            {
                case OverflowStyle.Ellipses:
                    lines[0] = Text[..((int)Width - 3)] + "...";
                    break;
                case OverflowStyle.Cutoff:
                    lines[0] = Text[..(int)Width];
                    break;
            }
        }

        for (var i = 0; i < Height; i++)
        {
            if (i < lines.Length)
            {
                canvas.Draw(X, Y + i, lines[i], TextColor, BackgroundColor);
            }
            
        }
    }
}

public enum OverflowStyle
{
    Ellipses,
    Cutoff
}

public enum TextAlign
{
    Left,
    Center,
    Right
}