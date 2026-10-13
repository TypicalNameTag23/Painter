using System.Runtime.InteropServices;
using Painter.Events;

namespace Painter.Platform;

internal class GenericTerminal : Terminal
{
    public override void Write(string text)
    {
        Console.Write(text);
    }

    public override KeyInfo? ReadKey()
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

        return new KeyInfo(new Key(key.Key, modifiers), key.KeyChar);
    }

    public override void Flush()
    {
        BufferStyleReset();
        Console.Write(Buffer);
        Buffer.Clear();
    }

    public override void PollSizeChange()
    {
        var width = Console.BufferWidth;
        var height = Console.BufferHeight;
        if (Width == width && Height == height)
        {
            return;
        }

        Width = width;
        Height = height;
        OnTerminalSizeChanged(this, new TerminalChangedSizeEventArgs(width, height));
    }
}