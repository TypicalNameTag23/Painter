using System.Runtime.InteropServices;
using Painter.Events;

namespace Painter.Platform;

internal class GenericTerminal : Terminal
{
    public override int Width => Console.BufferWidth;
    public override int Height => Console.BufferHeight;
    public override void Write(string text)
    {
        Console.Write(text);
    }

    public override Key? ReadKey()
    {
        if (!Console.KeyAvailable)
        {
            return null;
        }

        var key = Console.ReadKey(true);
        var modifiers = key.Modifiers;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Console.CapsLock)
        {
            modifiers ^= ConsoleModifiers.Shift;
        }
        
        return new Key(key.Key, key.KeyChar, modifiers);
    }

    public override void Flush()
    {
        ResetStyle();
        Console.Write(Buffer);
        Buffer.Clear();
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