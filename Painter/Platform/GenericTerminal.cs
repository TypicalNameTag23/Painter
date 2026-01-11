using Painter.Events;

namespace Painter.Platform;

internal class GenericTerminal : Terminal
{
    public override int Width => Console.BufferWidth;
    public override int Height => Console.BufferHeight;
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