using System.Drawing;
using Painter.Drawing;

namespace Painter.Elements;

// todo: dont allow strike through on empty space
public class TextBlock : Element
{
    public string Text
    {
        get => field;
        set => SetAndRedrawIfChanged(ref field, value);
    } = "";

    public TextAlignment TextAlignment
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = TextAlignment.Left;

    public bool WrapText
    {
        get => field;
        set
        {
            if (WrapText == value)
            {
                return;
            }

            field = value;
            Redraw();
        }
    } = true;

    public Color? TextColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    }

    public Color? BackgroundColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    }

    public OverflowStyle OverflowStyle
    {
        get; 
        set => SetAndRedrawIfChanged(ref field, value);
    } = OverflowStyle.Ellipses;
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        var width = Width;
        var height = Height;
        var text = Text;
        if (OverflowStyle == OverflowStyle.Ellipses)
        {
            var ellipsesStartIndex = width * height - 3;
            if (ellipsesStartIndex > 0 && text.Length > ellipsesStartIndex)
            {
                text = text[..ellipsesStartIndex] + "...";
            }
        }
        
        var lines = new string?[height];
        var lastLine = -1;
        var textIndex = 0;
        for (; lastLine + 1 < height && 
               textIndex < text.Length && 
               (WrapText || lastLine == -1); 
             textIndex += width, lastLine++)
        {
            var endIndex = textIndex + width;
            lines[lastLine + 1] = Align(Substring(text, textIndex, endIndex), width, TextAlignment);
        }
        
        for (var i = 0; i < lines.Length; i++)
        {
            var style = new Style
            {
                ForegroundColor = TextColor,
                BackgroundColor = BackgroundColor,
                Bold = true,
                // Strikethrough = true
            };
            canvas.Draw(X, Y + i, lines[i] is null ? new string(' ', width) : lines[i]!, style);
        }
    }

    private string Substring(string text, int from, int to)
    {
        return text[from..(to > text.Length ? text.Length : to)];
    }
    
    //todo: fix right align
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