using System.Drawing;
using System.Text;

namespace Painter.Drawing;

public class Canvas
{
    private readonly StringBuilder _buffer;
    private (int x, int y) _brushPosition = (0, 0);
    
    public int Width { get; private set; }
    
    public int Height { get; private set; }
    
    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;
        _buffer = new StringBuilder(width * height);
    }
    
    public void Draw(int x, int y, string text, Color? foregroundColor = null, Color? backgroundColor = null)
    {
        if (x >= Width || y >= Height)
        {
            return;
        }
        
        if (x < 0 && x + text.Length >= 0) // trim text to console line 
        {
            text = text[-x..];
        }

        if (x + text.Length > Width) // if placed off left side of console but some still shows up
        {
            text = text[..(Width - x)];
        }

        if (_brushPosition != (x, y))
        {
            _brushPosition = (x, y);
            _buffer.Append(AnsiTerminalCodes.SetCursorPosition(x, y));
        }

        _buffer.Append(AnsiTerminalCodes.SetForegroundColor(foregroundColor) +
                       AnsiTerminalCodes.SetBackgroundColor(backgroundColor) +
                       text +
                       AnsiTerminalCodes.SetForegroundColor(null) +
                       AnsiTerminalCodes.SetBackgroundColor(null));
        _brushPosition.x += text.Length;
    }

    public void Display()
    {
        Console.Write(_buffer);
        _buffer.Clear();
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