using System.Runtime.InteropServices;
using System.Text;
using Painter.Drawing;
using Painter.Events;

namespace Painter.Platform;

public abstract class Terminal
{
    private StrokeStyle _currentStrokeStyle = new();
    private int _cursorX;
    private int _cursorY;
    
    protected readonly StringBuilder Buffer = new(AnsiTerminalCodes.ResetCursorPosition);

    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public event TerminalSizeChangedEvent? TerminalSizeChangedEvent;

    public static Terminal GetPlatformTerminal()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsTerminal();
        }

        return new GenericTerminal();
    }

    public abstract void Write(string text);

    public abstract KeyInfo? ReadKey();
    
    public void WriteBuffered(char character)
    {
        Buffer.Append(character);
        _cursorX += 1;
    }
    
    public void SetCursorPosition(Point point)
    {
        if (_cursorX == point.X && _cursorY == point.Y)
        {
            return;
        }

        Buffer.Append(AnsiTerminalCodes.SetCursorPosition(point.X, point.Y));
        _cursorX = point.X;
        _cursorY = point.Y;
    }

    public void SetStyle(StrokeStyle strokeStyle)
    {
        if (_currentStrokeStyle.ForegroundColor != strokeStyle.ForegroundColor)
        {
            Buffer.Append(AnsiTerminalCodes.SetForegroundColor(strokeStyle.ForegroundColor));
        }
        
        if (_currentStrokeStyle.BackgroundColor != strokeStyle.BackgroundColor)
        {
            Buffer.Append(AnsiTerminalCodes.SetBackgroundColor(strokeStyle.BackgroundColor));
        }
        
        if (_currentStrokeStyle.Bold != strokeStyle.Bold)
        {
            Buffer.Append(AnsiTerminalCodes.SetBoldMode(strokeStyle.Bold));
        }
        
        if (_currentStrokeStyle.Italic != strokeStyle.Italic)
        {
            Buffer.Append(AnsiTerminalCodes.SetItalicsMode(strokeStyle.Italic));
        }
        
        if (_currentStrokeStyle.Strikethrough != strokeStyle.Strikethrough)
        {
            Buffer.Append(AnsiTerminalCodes.SetStrikethroughMode(strokeStyle.Strikethrough));
        }
        
        if (_currentStrokeStyle.Underline != strokeStyle.Underline)
        {
            Buffer.Append(AnsiTerminalCodes.SetUnderlineMode(strokeStyle.Underline));
        }
        
        if (_currentStrokeStyle.Blinking != strokeStyle.Blinking)
        {
            Buffer.Append(AnsiTerminalCodes.SetBlinkingMode(strokeStyle.Blinking));
        }

        _currentStrokeStyle = strokeStyle;
    }

    public void BufferStyleReset()
    {
        Buffer.Append(AnsiTerminalCodes.ResetStyles);
        _currentStrokeStyle = new StrokeStyle();
    }

    public abstract void Flush();

    public abstract void PollSizeChange();

    protected void OnTerminalSizeChanged(object sender, TerminalChangedSizeEventArgs args)
    {
        TerminalSizeChangedEvent?.Invoke(sender, args);
    }
}