using System.Drawing;
using System.Text;
using Painter.Platform;

namespace Painter.Drawing;

public class Canvas
{
    private readonly StringBuilder _buffer;
    private (int x, int y) _brushPosition = (0, 0);
    private FrameCache _frameCache;
    
    public int Width { get; private set; }
    
    public int Height { get; private set; }
    
    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;
        _buffer = new StringBuilder(Width * Height);
        _frameCache = new FrameCache(Width, Height);
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
        
        _brushPosition.x += text.Length;
        for (var i = 0; i < text.Length; i++)
        {
            if (_brushPosition != (x + i, y))
            {
                _brushPosition = (x, y);
                _buffer.Append(AnsiTerminalCodes.SetCursorPosition(x, y));
            }

            var currentPixel = _frameCache.GetCachedPixel(x + i, y);
            if (currentPixel is null && foregroundColor is not null || 
                currentPixel is not null && currentPixel.Value.foregroundColor != foregroundColor)
            {
                _buffer.Append(AnsiTerminalCodes.SetForegroundColor(foregroundColor));
            }

            if (currentPixel is null && backgroundColor is not null || 
                currentPixel is not null && currentPixel.Value.backgroundColor != backgroundColor)
            {
                _buffer.Append(AnsiTerminalCodes.SetBackgroundColor(backgroundColor));
            }

            if (currentPixel is null || currentPixel.Value.character != text[i])
            {
                _buffer.Append(text[i]);
            }

            _brushPosition.x++;
            _buffer.Append(AnsiTerminalCodes.SetForegroundColor(null) +
                           AnsiTerminalCodes.SetBackgroundColor(null));
        }
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
        _frameCache.Resize(Width, Height);
    }
    
}