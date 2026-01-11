using System.Drawing;
using Painter.Drawing;
using Painter.Drawing.Canvas;
using Painter.Utilities;

namespace Painter.Drawables;

public class TextBlock : Drawable
{
    public string Text
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = "";

    public Alignment Alignment
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = Alignment.Left;

    public bool WrapText
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = true;

    public Color? TextColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    }

    public virtual Color? BackgroundColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    }

    public OverflowStyle OverflowStyle
    {
        get; 
        set => SetAndRedrawIfChanged(ref field, value);
    } = OverflowStyle.Ellipses;
    
    public override void Draw(ICanvas canvas)
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
            lines[lastLine + 1] = text.SubstringSafely(textIndex, endIndex).Align(width, Alignment);
        }
        
        for (var i = 0; i < lines.Length; i++)
        {
            var style = new Brush
            {
                ForegroundColor = TextColor,
                BackgroundColor = BackgroundColor,
            };
            canvas.Draw(X, Y + i, lines[i] is null ? new string(' ', width) : lines[i]!, style);
        }
    }
}