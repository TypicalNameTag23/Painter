using System.Drawing;
using Taui.Exceptions;

namespace Taui.Drawing;

/// <summary>
/// Represents a <see cref="Canvas"/> that can be drawn to.
/// </summary>
public class Canvas
{
    private Pixel?[,] _pixelArray;
    
    /// <summary>
    /// The width of this canvas.
    /// </summary>
    public int Width { get; }
    
    /// <summary>
    /// The height of this canvas.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Constructs a new <see cref="Canvas"/>.
    /// </summary>
    /// <param name="width">The width of this canvas.</param>
    /// <param name="height">The height of this canvas.</param>
    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;
        _pixelArray = new Pixel[Width, Height];
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _pixelArray[x, y] = null;
            }
        }
    }
    
    /// <summary>
    /// Draw to a pixel on this canvas.
    /// </summary>
    /// <param name="x">The x coord to draw to.</param>
    /// <param name="y">The y coord to draw to.</param>
    /// <param name="pixel">The <see cref="Pixel"/> to draw to this canvas.</param>
    /// <exception cref="TauiException">Thrown when trying to draw to a pixel outside of the <see cref="Canvas"/>.</exception>
    public void Draw(int x, int y, Pixel pixel)
    {
        if (x >= Width || y >= Height)
        {
            throw new TauiException($"Trying to draw to pixel outside of the canvas (x: {x}, y: {y}) (width: {Width}, height: {Height})");
        }
        
        _pixelArray[x, y] = pixel;
    }

    /// <summary>
    /// Draw to a pixel on this canvas.
    /// </summary>
    /// <param name="x">The x coord to draw to.</param>
    /// <param name="y">The y coord to draw to.</param>
    /// <param name="character">The character to draw to the pixel.</param>
    /// <param name="foregroundColor">The color of the character.</param>
    /// <exception cref="TauiException">Thrown when trying to draw to a pixel outside of the <see cref="Canvas"/>.</exception>
    public void Draw(int x, int y, char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        if (x >= Width || y >= Height)
        {
            throw new TauiException($"Trying to draw to pixel outside of the canvas (x: {x}, y: {y}) (width: {Width}, height: {Height})");
        }
        
        _pixelArray[x, y] = new Pixel(character)
        {
            ForegroundColor = foregroundColor,
            BackgroundColor = backgroundColor
        };
    }

    /// <summary>
    /// Draw to a pixel on this canvas.
    /// </summary>
    /// <param name="x">The x coord to draw to.</param>
    /// <param name="y">The y coord to draw to.</param>
    /// <param name="pixels"></param>
    /// <exception cref="TauiException">Thrown when trying to draw to a pixel outside of the <see cref="Canvas"/>.</exception>
    public void Draw(int x, int y, Pixel[,] pixels)
    {
        if (x >= Width || y >= Height)
        {
            throw new TauiException($"Trying to draw to pixel outside of the canvas (x: {x}, y: {y}) (width: {Width}, height: {Height})");
        }

        for (; y < pixels.GetLength(1); y++)
        {
            for (; x < pixels.GetLength(0); x++)
            {
                Draw(x, y, pixels[x, y]);
            }
        }
    }

    /// <summary>
    /// View a pixel on this canvas.
    /// </summary>
    /// <param name="x">The x coord to view.</param>
    /// <param name="y">The y coord to view.</param>
    /// <returns>The pixel at the specified coords.</returns>
    /// <exception cref="TauiException">Thrown when trying to view a pixel outside of the <see cref="Canvas"/></exception>
    public Pixel View(int x, int y)
    {
        if (x >= Width || y >= Height)
        {
            throw new TauiException($"Trying to view pixel outside of the canvas (x: {x}, y: {y}) (width: {Width}, height: {Height})");
        }
        
        return _pixelArray[x, y];
    }

    /// <summary>
    /// Clears this <see cref="Canvas"/>
    /// </summary>
    public void Clear()
    {
        Array.Clear(_pixelArray);
    }

    /// <summary>
    /// Runs the provided function on all pixels in this <see cref="Canvas"/>.
    /// </summary>
    /// <param name="func">The function to run</param>
    public void RunOnPixels(Action<int, int, Pixel> func)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                func(x, y, _pixelArray[x, y]);
            }
        }
    }
}