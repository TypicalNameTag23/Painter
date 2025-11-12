using System.Drawing;
using System.Text;
using Painter.Rendering;

namespace Painter.Drawing;

public class Canvas
{
    private readonly StringBuilder _buffer;
    private Color? _currentForegroundColor;
    private Color? _currentBackgroundColor;
    private (int x, int y) _brushPosition = (0, 0);
    
    public int Width { get; private set; }
    
    public int Height { get; private set; }
    
    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;
        _buffer = new StringBuilder(width * height);
    }
    
    public void Draw(int x, int y, string text, Color? backgroundColor, Color? foregroundColor)
    {
        if (x >= Width || y >= Height)
        {
            return;
        }
        
        if (x < 0 && x + text.Length >= 0)
        {
            text = text[-x..];
        }

        if (x + text.Length > Width)
        {
            text = text[..(Width - x)];
        }

        if (_brushPosition != (x, y))
        {
            _brushPosition = (x, y);
            _buffer.Append(AnsiTerminalCodes.SetCursorPosition(x, y));
        }
        
        if (_currentForegroundColor != foregroundColor)
        {
            _currentForegroundColor = foregroundColor;
            _buffer.Append(AnsiTerminalCodes.SetForegroundColor(foregroundColor));
        }

        if (_currentBackgroundColor != backgroundColor)
        {
            _currentBackgroundColor = backgroundColor;
            _buffer.Append(AnsiTerminalCodes.SetBackgroundColor(backgroundColor));
        }

        _buffer.Append(text);
        _brushPosition.x += text.Length;
    }

    public void Display()
    {
        _buffer.Append(AnsiTerminalCodes.Reset);
        Console.Write(_buffer);
        _currentForegroundColor = null;
        _currentBackgroundColor = null;
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

    private bool IsOutsideCanvas(uint x, uint y)
    {
        return x >= Width || y >= Height;
    }
    
}