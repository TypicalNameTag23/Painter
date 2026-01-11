using Painter.Platform;

namespace Painter.Drawing;

public class Canvas
{
    private readonly Terminal _terminal;
    private Dictionary<(int, int), (char, Style)> _pixels = [];

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Canvas(Terminal terminal, int width, int height)
    {
        Width = width;
        Height = height;
        _terminal = terminal;
    }

    public void Draw(int x, int y, char character, Style style)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            return;
        }

        _pixels[(x, y)] = (character, style);
    }

    public void Draw(int x, int y, Style style)
    {
        Draw(x, y, ' ', style);
    }
    
    // todo: handle newline characters
    public void Draw(int x, int y, object obj, Style style)
    {
        // // make sure we aren't trying to draw out of the bounds of the canvas
        // if (x >= Width || y >= Height || y < 0)
        // {
        //     return;
        // }
        //
        // // if the thing being drawn is placed off the left side of the canvas but still has some text that shows up 
        // if (x < 0 && x + text.Length >= 0)
        // {
        //     // trim the text to the parts that would be on the canvas
        //     text = text[-x..];
        //     x = 0;
        // }
        //
        // // trim text to canvas' area if it goes outside the right side of the canvas
        // if (x + text.Length > Width)
        // {
        //     text = text[..(Width - x)];
        // }
        var text = obj.ToString();
        for (var i = 0; i < text?.Length; i++)
        {
            Draw(x + i, y, text[i], style);
        }
    }

    public void Display()
    {
        foreach (var ((x, y), (character, style)) in _pixels)
        {
            _terminal.SetCursorPosition(x, y);
            _terminal.SetStyle(style);
            _terminal.Write(character);
        }

        _pixels.Clear();
        _terminal.Flush();
    }

    internal void Resize(int width, int height)
    {
        if (Width <= 0 || Height <= 0)
        {
            return;
        }
        
        _pixels.Clear();
        Width = width;
        Height = height;
    }
    
}