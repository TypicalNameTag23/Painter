using System.Runtime.InteropServices;
using System.Text;
using Painter.Drawing;

namespace Painter.Platform;

public abstract class Terminal
{
    private readonly StringBuilder _buffer = new();
    private Style _currentStyle = new();
    private int _cursorX;
    private int _cursorY;

    public int Width => Console.BufferWidth;
    public int Height => Console.BufferHeight;

    public static Terminal GetPlatformTerminal()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsTerminal();
        }

        return new GenericTerminal();
    }
    
    public Terminal Write(string text)
    {
        _buffer.Append(text);

        var newX = _cursorX + text.Length;
        if (newX > Width)
        {
            _cursorY = newX / Width;
            newX %= Width; // guh
        }
        
        _cursorX += newX;
        return this;
    }
    
    public Terminal Write(char character)
    {
        Write(character.ToString());
        return this;
    }

    public Terminal SetCursorPosition(int x, int y)
    {
        if (_cursorX == x && _cursorY == y)
        {
            return this;
        }

        _buffer.Append(AnsiTerminalCodes.SetCursorPosition(x, y));
        return this;
    }

    public Terminal SetStyle(Style style)
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

        return this;
    }

    public Terminal ResetStyle()
    {
        // _buffer.Append(AnsiTerminalCodes.ResetStyles);
        _buffer.Append(AnsiTerminalCodes.ResetBackgroundColor);
        _buffer.Append(AnsiTerminalCodes.ResetForegroundColor);
        _currentStyle = new Style();

        return this;
    }

    public Terminal Flush()
    {
        ResetStyle();
        Console.Write(_buffer);
        _buffer.Clear();

        return this;
    }
}