using System.Runtime.InteropServices;
using Painter.Events;

namespace Painter.Platform;

internal class WindowsTerminal : Terminal
{
    const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
    
    [DllImport("kernel32.dll")]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    
    public WindowsTerminal()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(handle, out var mode);
        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        SetConsoleMode(handle, mode);
    }

    public override int Width => Console.BufferWidth;
    public override int Height => Console.BufferHeight;
    
    public override void Flush()
    {
        ResetStyle();
        Console.Write(Buffer);
        Buffer.Clear();
    }
    
    public override Key? ReadKey()
    {
        if (!Console.KeyAvailable)
        {
            return null;
        }

        var key = Console.ReadKey(true);
        var modifiers = key.Modifiers;
        if (Console.CapsLock)
        {
            modifiers ^= ConsoleModifiers.Shift;
        }
        
        return new Key(key.Key, key.KeyChar, modifiers);
    }
    
    public override void Write(string text)
    {
        Console.Write(text);
    }

    public override void PollSizeChange()
    {
        var width = Width;
        var height = Height;
        if (Width == PreviousWidth && Height == PreviousHeight)
        {
            return;
        }

        PreviousWidth = width;
        PreviousHeight = height;
        OnTerminalSizeChanged(this, new TerminalChangedSizeEventArgs(width, height));
    }
}