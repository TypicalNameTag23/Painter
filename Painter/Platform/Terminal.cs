using System.Runtime.InteropServices;
using System.Text;
using Painter.Drawing;
using Painter.Events;

namespace Painter.Platform;

internal abstract class Terminal
{
    private StrokeStyle _currentStrokeStyle = new();
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

    public virtual void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        
        Console.Write(
            // AnsiTerminalCodes.FuckAssLinuxThingStolenFromTerminalDotGui +
            AnsiTerminalCodes.DisableLineWrapping +
            AnsiTerminalCodes.SaveScreen + 
            AnsiTerminalCodes.SaveCursorPositon + 
            AnsiTerminalCodes.ClearScreen +
            AnsiTerminalCodes.HideCursor +
            // AnsiTerminalCodes.CSI_EnableAnyEventMouse +
            // AnsiTerminalCodes.CSI_EnableSgrExtModeMouse +
            // AnsiTerminalCodes.CSI_EnableUrxvtExtModeMouse);
            AnsiTerminalCodes.ResetCursorPosition + 
            AnsiTerminalCodes.ResetStyles);
        
        Console.TreatControlCAsInput = true;
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

    public void ResetStyle()
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