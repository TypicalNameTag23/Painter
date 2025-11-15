using Painter.Platform;

namespace Painter.Drawing;

public class Canvas
{
    private readonly Terminal _terminal;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Canvas(Terminal terminal, int width, int height)
    {
        Width = width;
        Height = height;
        _terminal = terminal;
    }

#if DEBUG
    public static event FrameRenderEventHandler? DrawStart;
    public static event FrameRenderEventHandler? DrawEnd;
#endif
    
    // todo: handle newline characters
    public void Draw(int x, int y, string text, Style style)
    {
#if DEBUG
        DrawStart?.Invoke();
#endif
        // make sure we aren't trying to draw out of the bounds of the canvas
        if (x >= Width || y >= Height || y < 0)
        {
            return;
        }

        // if the thing being drawn is placed off the left side of the canvas but still has some text that shows up 
        if (x < 0 && x + text.Length >= 0)
        {
            // trim the text to the parts that would be on the canvas
            text = text[-x..];
        }

        // trim text to canvas' area if it goes outside the right side of the canvas
        if (x + text.Length > Width)
        {
            text = text[..(Width - x)];
        }
        
        _terminal.SetCursorPosition(x, y);
        _terminal.SetStyle(style);
        _terminal.Write(text);
#if DEBUG
        DrawEnd?.Invoke();
#endif
    }

    public void Display()
    {
        _terminal.Flush();
    }

    internal void Resize(int width, int height)
    {
        if (Width <= 0 || Height <= 0)
        {
            return;
        }
        
        Width = width;
        Height = height;
    }
    
}