using Painter.Events;
using Painter.Platform;

namespace Painter.Drawing.Canvas;

/// <summary>
/// A canvas that can print its content to a <see cref="Terminal"/>
/// </summary>
internal class TerminalCanvas : Canvas
{
    private Dictionary<Point, Pixel> _previousFrame = [];
    private Dictionary<Point, Pixel> _pixels = [];
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

    public override void Draw(int x, int y, char character, StrokeStyle strokeStyle)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            return;
        }

        var point = new Point(x, y);
        if (strokeStyle.BackgroundColor == Color.Transparent && _pixels.TryGetValue(point, out var pixel))
        {
            strokeStyle = new StrokeStyle
            {
                BackgroundColor = pixel.StrokeStyle?.BackgroundColor,
                ForegroundColor = strokeStyle.ForegroundColor,
                Blinking = strokeStyle.Blinking,
                Bold = strokeStyle.Bold,
                Italic = strokeStyle.Italic,
                Strikethrough = strokeStyle.Strikethrough,
                Underline = strokeStyle.Underline
            };
        }
        
        _pixels[point] = new Pixel(
            character == ' ' 
                ? null 
                : character, 
            strokeStyle == StrokeStyle.Unstyled
                ? null 
                : strokeStyle);
    }

    /// <summary>
    /// Displays this <see cref="TerminalCanvas"/> to the provided <see cref="Terminal"/>
    /// </summary>
    internal void Display()
    {
        foreach (var (point, pixel) in _pixels)
        {
            // If there was a pixel there in the old frame remove it
            if (_previousFrame.Remove(point, out var oldPixel))
            {
                // If the old pixel matches the current one we are drawing, continue instead of redrawing it
                if (oldPixel.Character == pixel.Character && oldPixel.StrokeStyle == pixel.StrokeStyle)
                {
                    continue;
                }
            }

            // Redraw the pixel if it has changed
            _terminal.SetCursorPosition(point);
            _terminal.SetStyle(pixel.StrokeStyle ?? StrokeStyle.Unstyled);
            _terminal.WriteBuffered(pixel.Character ?? ' ');
        }
        
        // All remaining pixels that haven't been removed were not drawn to in the new frame
        foreach (var (point, _) in _previousFrame)
        {
            _terminal.SetCursorPosition(point);
            // This means that they should be blank
            _terminal.SetStyle(StrokeStyle.Unstyled);
            _terminal.WriteBuffered(' ');
        }
        
        _previousFrame = _pixels;
        // todo: Is there a better way to do this other than creating a new Dictionary every frame?
        _pixels = [];
        _terminal.Flush();
    }

    private void OnTerminalChangedSize(object? sender, TerminalChangedSizeEventArgs args)
    {
        _pixels.Clear();
        _previousFrame.Clear();
        Width = args.Width;
        Height = args.Height;
    }
}