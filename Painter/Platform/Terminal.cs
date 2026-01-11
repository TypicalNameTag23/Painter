using System.Runtime.InteropServices;
using System.Text;
using Painter.Drawing;

namespace Painter.Platform;

internal abstract class Terminal
{
    private readonly StringBuilder _buffer = new(AnsiTerminalCodes.ResetCursorPosition);
    private Style _currentStyle = new();
    private int _cursorX;
    private int _cursorY;

    public static int Width => Console.BufferWidth;
    public static int Height => Console.BufferHeight;

    public static Terminal GetPlatformTerminal()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsTerminal();
        }

        return new GenericTerminal();
    }
    
    public void Write(string text)
    {
        _buffer.Append(text);
        _cursorX += text.Length;
    }
    
    public void Write(char character)
    {
        _buffer.Append(character);
        _cursorX += 1;
    }
    
    public void SetCursorPosition(int x, int y)
    {
        if (_cursorX == x && _cursorY == y)
        {
            return;
        }

        _buffer.Append(AnsiTerminalCodes.SetCursorPosition(x, y));
        _cursorX = x;
        _cursorY = y;
    }

    public void SetStyle(Style style)
    {
        // lol trash
        if (_currentStyle.ForegroundColor != style.ForegroundColor)
        {
            _buffer.Append(AnsiTerminalCodes.SetForegroundColor(style.ForegroundColor));
        }
        
        if (_currentStyle.BackgroundColor != style.BackgroundColor)
        {
            _buffer.Append(AnsiTerminalCodes.SetBackgroundColor(style.BackgroundColor));
        }
        
        if (_currentStyle.Bold != style.Bold)
        {
            _buffer.Append(AnsiTerminalCodes.SetBoldMode(style.Bold));
        }
        
        if (_currentStyle.Italic != style.Italic)
        {
            _buffer.Append(AnsiTerminalCodes.SetItalicsMode(style.Italic));
        }
        
        if (_currentStyle.Strikethrough != style.Strikethrough)
        {
            _buffer.Append(AnsiTerminalCodes.SetStrikethroughMode(style.Strikethrough));
        }
        
        if (_currentStyle.Underline != style.Underline)
        {
            _buffer.Append(AnsiTerminalCodes.SetUnderlineMode(style.Underline));
        }

        _currentStyle = style;
    }

    public void ResetStyle()
    {
        _buffer.Append(AnsiTerminalCodes.ResetStyles);
        _buffer.Append(AnsiTerminalCodes.ResetBackgroundColor);
        _buffer.Append(AnsiTerminalCodes.ResetForegroundColor);
        _currentStyle = new Style();
    }

    public void Flush()
    {
        ResetStyle();
        Console.Write(_buffer);
        _buffer.Clear();
    }
}