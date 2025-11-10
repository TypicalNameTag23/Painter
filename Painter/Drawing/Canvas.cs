using Painter.Exceptions;

namespace Painter.Drawing;

/// <summary>
/// Represents a <see cref="Canvas"/> that can be drawn to.
/// </summary>
public class Canvas // todo: add stuff to allow for relative positioning 
{
    private readonly Dictionary<Point, Pixel> _drawnPixels = [];
    
    /// <summary>
    /// The width of this <see cref="Canvas"/>.
    /// </summary>
    public int Width { get; private set; }
    
    /// <summary>
    /// The height of this <see cref="Canvas"/>.
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Constructs a new <see cref="Canvas"/>.
    /// </summary>
    /// <param name="width">The width of this canvas.</param>
    /// <param name="height">The height of this canvas.</param>
    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Draw to a pixel on this canvas.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to draw the pixel at.</param>
    /// <param name="pixel">The <see cref="Pixel"/> to draw to this canvas.</param>
    /// <exception cref="PainterException">Thrown when trying to draw to a <see cref="Point"/> outside this <see cref="Canvas"/>.</exception>
    public void Draw(Point point, Pixel pixel)
    {
        if (IsPointOutsideCanvas(point))
        {
            throw new PainterException($"Trying to draw to pixel outside of the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
        }

        _drawnPixels[point] = pixel;
    }
    
    /// <summary>
    /// Erases a <see cref="Pixel"/> from this <see cref="Canvas"/>.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to erase a <see cref="Pixel"/> from.</param>
    /// <exception cref="PainterException">Thrown when trying to erase from a <see cref="Point"/> outside this <see cref="Canvas"/>.</exception>
    public void Erase(Point point)
    {
        if (IsPointOutsideCanvas(point))
        {
            throw new PainterException($"Trying to draw to pixel outside of the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
        }

        _drawnPixels.Remove(point);
    }

    /// <summary>
    /// View a <see cref="Pixel"/> on this <see cref="Canvas"/>.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to view the <see cref="Pixel"/> at.</param>
    /// <returns>The <see cref="Pixel"/> at the specified <see cref="Point"/>, null if there isn't one.</returns>
    /// <exception cref="PainterException">Thrown when trying to view a <see cref="Pixel"/> at a <see cref="Point"/> outside this <see cref="Canvas"/>.</exception>
    public Pixel? View(Point point)
    {
        if (IsPointOutsideCanvas(point))
        {
            throw new PainterException($"Trying to view pixel outside of the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
        }
        
        return _drawnPixels.GetValueOrDefault(point);
    }
    
    /// <summary>
    /// Resizes this <see cref="Canvas"/>.
    /// </summary>
    /// <param name="width">The new width, greater than 0.</param>
    /// <param name="height">The new height, greater than 0.</param>
    /// <exception cref="PainterException">Thrown when trying to resize this <see cref="Canvas"/> to a value less than 1.</exception>
    public void Resize(int width, int height)
    {
        if (Width <= 0 || Height <= 0) // Should use setters for this?
        {
            throw new PainterException($"Canvas size can not be less than 0 (width: {width}, height: {height}).");
        }
        
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Clears this <see cref="Canvas"/>
    /// </summary>
    public void Clear()
    {
        _drawnPixels.Clear();
    }
    
    /// <summary>
    /// Runs the provided function on all the drawn to pixels in this <see cref="Canvas"/>.
    /// </summary>
    /// <param name="func">The function to run</param>
    public void RunOnDrawnPixels(Action<Point, Pixel> func)
    {
        foreach (var pointPixelPair in _drawnPixels)
        {
            func(pointPixelPair.Key, pointPixelPair.Value);
        }
    }

    private bool IsPointOutsideCanvas(Point point)
    {
        return point.X >= Width || point.Y >= Height || point.X < 0 || point.Y < 0;
    }
}

    // Move to extensions class 
    // /// <summary>
    // /// Draw to a pixel on this canvas.
    // /// </summary>
    // /// <param name="x">The x coord to draw to.</param>
    // /// <param name="y">The y coord to draw to.</param>
    // /// <param name="character">The character to draw to the pixel.</param>
    // /// <param name="foregroundColor">The color of the character.</param>
    // /// <exception cref="TauiException">Thrown when trying to draw to a pixel outside the <see cref="Canvas"/>.</exception>
    // public void Draw(Point point, char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    // {
    //     if (x >= Width || y >= Height)
    //     {
    //         throw new TauiException($"Trying to draw to pixel outside the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
    //     }
    //
    //     var point = new Point(x, y);
    //     var pixel = new Pixel(character)
    //     {
    //         ForegroundColor = foregroundColor,
    //         BackgroundColor = backgroundColor
    //     };
    //     
    //     _drawnPixels[point] = pixel;
    // }
    //
    // /// <summary>
    // /// Draw to a pixel on this canvas.
    // /// </summary>
    // /// <param name="x">The x coord to draw to.</param>
    // /// <param name="y">The y coord to draw to.</param>
    // /// <param name="pixels"></param>
    // /// <exception cref="TauiException">Thrown when trying to draw to a pixel outside the <see cref="Canvas"/>.</exception>
    // public void Draw(Point point, Pixel[,] pixels)
    // {
    //     if (x >= Width || y >= Height)
    //     {
    //         throw new TauiException($"Trying to draw to pixel outside the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
    //     }
    //
    //     for (; y < pixels.GetLength(1); y++)
    //     {
    //         for (; x < pixels.GetLength(0); x++)
    //         {
    //             Draw(x, y, pixels[x, y]);
    //         }
    //     }
    // }