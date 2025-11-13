using System.Drawing;
using Painter.Drawing;

namespace Painter.Widgets;

public class TextBlock : Widget
{
    public string Text { get; set; } = "";

    public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
    
    public bool WrapText { get; set; } = true;
    
    public Color? TextColor { get; set; }
    
    public Color? BackgroundColor { get; set; }
    
    public OverflowStyle OverflowStyle { get; set; } = OverflowStyle.Ellipses;
    
    public override void Draw(Canvas canvas)
    {
        if (Width is null || Height is null)
        {
            return;
        }

        var lastLine = 0;
        var width = (int)Width;
        var height = (int)Height;
        var lines = new string?[height];
        lines[0] = Align(Substring(Text, 0, width), width, TextAlignment);
        if (WrapText)
        {
            var textIndex = width;
            for (; textIndex < Text.Length && lastLine + 1 < height; textIndex += width, lastLine++)
            {
                var endIndex = textIndex + width;
                lines[lastLine + 1] = Align(Substring(Text, textIndex, endIndex > Text.Length ? Text.Length - 1 : textIndex + width), width, TextAlignment);
            }
        }

        if (Text.Length > width * height || !WrapText && Text.Length > width)
        {
            lines[^1] = OverflowStyle switch
            {
                OverflowStyle.Ellipses => $"{lines[lastLine]?[..(width - 3)]}...",
                OverflowStyle.Cutoff => lines[lastLine]?[..width]
            };
        }

        for (var i = 0; i < lines.Length; i++)
        {
            canvas.Draw(X, Y + i, lines[i] is null ? new string(' ', width) : lines[i]!, TextColor, BackgroundColor);
        }
    }

    private string Substring(string text, int from, int to)
    {
        return Text[from..(to > Text.Length ? Text.Length - 1 : to)];
    }
    
    private string Align(string text, int width, TextAlignment alignment)
    {
        switch (alignment)
        {
            case TextAlignment.Right:
            {
                return text.PadLeft(width, ' ');
            }

            case TextAlignment.Center:
            {
                var leftPadding = width / 2 + text.Length / 2;
                text = text.PadLeft(leftPadding, ' ');
                text = text.PadRight(width, ' ');
                return text;
            }

            case TextAlignment.Left:
            {
                return text.PadRight(width, ' ');
            }
        }

        return text;
    }
}

public enum OverflowStyle
{
    Ellipses,
    Cutoff
}

public enum TextAlignment
{
    Left,
    Center,
    Right
}