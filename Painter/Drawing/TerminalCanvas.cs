using Painter.Platform;

namespace Painter.Drawing;

public class TerminalCanvas : ICanvas
{
    private readonly Dictionary<(int, int), (char, Style)> _pixels;
    private readonly Terminal _terminal;

    internal TerminalCanvas(Terminal terminal, int width, int height)
    {
        Width = width;
        Height = height;
        _terminal = terminal;
        _pixels = [];
    }
    
    public int Width { get; set; }
    
    public int Height { get; set; }

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
    
    public void Draw(int x, int y, object obj, Style style)
    {
        var text = obj.ToString();
        text = text?.Replace(Environment.NewLine, "");
        for (var i = 0; i < text?.Length; i++)
        {
            Draw(x + i, y, text[i], style);
        }
    }

    public ICanvas Subsection(int x, int y, int width, int height)
    {
        return new CanvasSubsection(x, y, width, height, this);
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