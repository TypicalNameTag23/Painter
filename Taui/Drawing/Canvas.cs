using Taui.Exceptions;

namespace Taui.Drawing;

/// <summary>
/// Represents a <see cref="Canvas"/> that can be drawn to.
/// </summary>
public class Canvas
{
    private readonly Dictionary<Point, Pixel> _drawnPixels = [];
    
    /// <summary>
    /// The width of this canvas.
    /// </summary>
    public int Width { get; private set; }
    
    /// <summary>
    /// The height of this canvas.
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
    /// <param name="point"></param>
    /// <param name="pixel">The <see cref="Pixel"/> to draw to this canvas.</param>
    /// <exception cref="TauiException">Thrown when trying to draw to a pixel outside the <see cref="Canvas"/>.</exception>
    public void Draw(Point point, Pixel pixel)
    {
        if (point.X >= Width || point.Y >= Height)
        {
            throw new TauiException($"Trying to draw to pixel outside of the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
        }

        _drawnPixels[point] = pixel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    /// <exception cref="TauiException"></exception>
    public void Erase(Point point)
    {
        if (point.X >= Width || point.Y >= Height)
        {
            throw new TauiException($"Trying to draw to pixel outside of the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
        }

        _drawnPixels.Remove(point);
    }

    /// <summary>
    /// View a pixel on this canvas.
    /// </summary>
    /// <param name="point"></param>
    /// <returns>The pixel at the specified coords.</returns>
    /// <exception cref="TauiException">Thrown when trying to view a pixel outside the <see cref="Canvas"/></exception>
    public Pixel? View(Point point)
    {
        if (point.X >= Width || point.Y >= Height)
        {
            throw new TauiException($"Trying to view pixel outside of the canvas (x: {point.X}, y: {point.Y}) (width: {Width}, height: {Height})");
        }
        
        return _drawnPixels.GetValueOrDefault(point);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void Resize(int width, int height)
    {
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
    /// Runs the provided function on all pixels in this <see cref="Canvas"/>.
    /// </summary>
    /// <param name="func">The function to run</param>
    public void RunOnPixels(Action<Point, Pixel> func)
    {
        foreach (var pointPixelPair in _drawnPixels)
        {
            func(pointPixelPair.Key, pointPixelPair.Value);
        }
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