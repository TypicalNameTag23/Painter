using Painter.Events;
using Painter.Platform;

namespace Painter.Drawing.Canvas;

/// <summary>
/// A canvas that can print its content to a <see cref="Terminal"/>
/// </summary>
public class TerminalCanvas : Canvas
{
    private readonly Dictionary<(int, int), (char, Brush)> _pixels = [];
    private readonly Terminal _terminal;

    /// <summary>
    /// Creates a new <see cref="TerminalCanvas"/>
    /// </summary>
    /// <param name="terminal">The <see cref="Terminal"/> to print to</param>
    /// <param name="width">The width of this canvas</param>
    /// <param name="height">The height of this canvas</param>
    internal TerminalCanvas(Terminal terminal, int width, int height)
    {
        Width = width;
        Height = height;
        _terminal = terminal;
        _terminal.TerminalSizeChangedEvent += OnTerminalChangedSize;
    }

    public override void Draw(int x, int y, char character, Brush brush)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            return;
        }

        _pixels[(x, y)] = (character, brush);
    }

    public override ICanvas Subsection(int x, int y, int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(width, Width);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(height, Height);
        
        return new CanvasSubsection(x, y, width, height, this);
    }

    /// <summary>
    /// Displays this <see cref="TerminalCanvas"/> to the provided <see cref="Terminal"/>
    /// </summary>
    internal void Display()
    {
        foreach (var ((x, y), (character, style)) in _pixels)
        {
            _terminal.SetCursorPosition(x, y);
            _terminal.SetStyle(style);
            _terminal.Write(character);
        }

        _pixels.Clear();
        _terminal.Flush();
    }

    private void OnTerminalChangedSize(object? sender, TerminalChangedSizeEventArgs args)
    {
        _pixels.Clear();
        Width = args.Width;
        Height = args.Height;
    }
}