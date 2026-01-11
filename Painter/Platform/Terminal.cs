using System.Runtime.InteropServices;
using System.Text;
using Painter.Drawing;
using Painter.Events;

namespace Painter.Platform;

internal abstract class Terminal
{
    private Brush _currentBrush = new();
    private bool _initialized;
    private int _cursorX;
    private int _cursorY;
    
    protected readonly StringBuilder Buffer = new(AnsiTerminalCodes.ResetCursorPosition);
    protected int PreviousWidth = 0;
    protected int PreviousHeight = 0;

    public abstract int Width { get; }
    public abstract int Height { get; }

    public event TerminalSizeChangedEvent TerminalSizeChangedEvent;

    public static Terminal GetPlatformTerminal()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsTerminal();
        }

        return new GenericTerminal();
    }

    public void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        
        PollSizeChange();
        _initialized = true;
    }

    public void Write(string text)
    {
        var sections = text.Split(Environment.NewLine);
        Buffer.Append(sections[0]);
        _cursorX += sections[0].Length;

        if (sections.Length == 1)
        {
            return;
        }
        
        foreach (var textSection in sections)
        {
            SetCursorPosition(0, _cursorY + 1);
            Buffer.Append(textSection);
            _cursorX += sections[0].Length;
        }
    }
    
    public void Write(char character)
    {
        Buffer.Append(character);
        _cursorX += 1;
    }
    
    public void SetCursorPosition(int x, int y)
    {
        if (_cursorX == x && _cursorY == y)
        {
            return;
        }

        Buffer.Append(AnsiTerminalCodes.SetCursorPosition(x, y));
        _cursorX = x;
        _cursorY = y;
    }

    public void SetStyle(Brush brush)
    {
        if (_currentBrush.ForegroundColor != brush.ForegroundColor)
        {
            Buffer.Append(AnsiTerminalCodes.SetForegroundColor(brush.ForegroundColor));
        }
        
        if (_currentBrush.BackgroundColor != brush.BackgroundColor)
        {
            Buffer.Append(AnsiTerminalCodes.SetBackgroundColor(brush.BackgroundColor));
        }
        
        if (_currentBrush.Bold != brush.Bold)
        {
            Buffer.Append(AnsiTerminalCodes.SetBoldMode(brush.Bold));
        }
        
        if (_currentBrush.Italic != brush.Italic)
        {
            Buffer.Append(AnsiTerminalCodes.SetItalicsMode(brush.Italic));
        }
        
        if (_currentBrush.Strikethrough != brush.Strikethrough)
        {
            Buffer.Append(AnsiTerminalCodes.SetStrikethroughMode(brush.Strikethrough));
        }
        
        if (_currentBrush.Underline != brush.Underline)
        {
            Buffer.Append(AnsiTerminalCodes.SetUnderlineMode(brush.Underline));
        }
        
        if (_currentBrush.Blinking != brush.Blinking)
        {
            Buffer.Append(AnsiTerminalCodes.SetBlinkingMode(brush.Blinking));
        }

        _currentBrush = brush;
    }

    public void ResetStyle()
    {
        Buffer.Append(AnsiTerminalCodes.ResetStyles);
        Buffer.Append(AnsiTerminalCodes.ResetBackgroundColor);
        Buffer.Append(AnsiTerminalCodes.ResetForegroundColor);
        _currentBrush = new Brush();
    }

    public abstract void Flush();

    public abstract void PollSizeChange();

    protected void OnTerminalSizeChanged(object sender, TerminalChangedSizeEventArgs args)
    {
        TerminalSizeChangedEvent?.Invoke(sender, args);
    }
}