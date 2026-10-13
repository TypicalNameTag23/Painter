using Painter.Drawing;
using Painter.Drawing.Canvas;
using Painter.Events;
using Painter.Utilities;

namespace Painter.Drawables;

public class TextBox : Drawable
{
    public override bool Focusable { get; set; } = true;

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

    public Color? TextColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    }

    public virtual Color? BackgroundColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = Color.Black;

    public virtual Color? FocusedColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = Color.Black;

    public OverflowStyle OverflowStyle
    {
        get; 
        set => SetAndRedrawIfChanged(ref field, value);
    } = OverflowStyle.Ellipses;

    public int CursorPosition
    {
        get;
        set
        {
            var position = value;
            if (position > Text.Length)
            {
                position = Text.Length;
            }
            else if (position < 0)
            {
                position = 0;
            }

            SetAndRedrawIfChanged(ref field, position);
        }
    }

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
               textIndex < text.Length; 
             textIndex += width, lastLine++)
        {
            var endIndex = textIndex + width;
            lines[lastLine + 1] = text.SubstringSafely(textIndex, endIndex).Align(width, Alignment);
        }
        
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i] is null ? new string(' ', width) : lines[i]!;
            for (var j = 0; j < line.Length; j++)
            {
                var style = j + i * width == CursorPosition
                    ? new StrokeStyle
                    {
                        ForegroundColor = TextColor,
                        BackgroundColor = Focused ? FocusedColor : BackgroundColor,
                        Underline = Focused
                    }
                    : new StrokeStyle
                    {
                        ForegroundColor = TextColor,
                        BackgroundColor = Focused ? FocusedColor : BackgroundColor,
                        Underline = false
                    };
                
                canvas.Draw(X + j, Y + i, line[j], style);
            }
        }
    }

    public override void Interact(KeyPressedEventArgs args)
    {
        args.Handled = true;
        switch (args.Key.KeyType)
        {
            case ConsoleKey.Backspace:
            {
                if (CursorPosition != 0)
                {
                    Text = Text.Remove(CursorPosition - 1, 1);
                    CursorPosition--;
                }
                
                return;
            }
            case ConsoleKey.Delete:
            {
                if (CursorPosition != Text.Length)
                {
                    Text = Text.Remove(CursorPosition, 1);
                }
                
                return;
            }
            case ConsoleKey.LeftArrow:
            {
                CursorPosition--;
                return;
            }
            case ConsoleKey.RightArrow:
            {
                CursorPosition++;
                return;
            }
            case ConsoleKey.UpArrow:
            {
                CursorPosition -= Width;
                return;
            }
            case ConsoleKey.DownArrow:
            {
                CursorPosition += Width;
                return;
            }
        }

        if (args.Character == '\0' || args.Key.KeyModifiers.HasFlag(ConsoleModifiers.Control))
        {
            args.Handled = false;
            return;
        }

        Text += args.Character;
        CursorPosition++;
    }
}