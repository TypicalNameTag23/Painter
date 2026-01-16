using System.ComponentModel;

namespace Painter.Drawing;

public readonly struct Rect(int x, int y, int width, int height) : IEquatable<Rect>
{
    public Rect(Size availableSize) : this(0, 0, availableSize.Width, availableSize.Height) { }

    public int X { get; } = x;

    public int Y { get; } = y;

    public int Width { get; } = width;

    public int Height { get; } = height;

    /// <summary>
    /// Deflates a rectangle by a certain <see cref="Thickness"/>
    /// </summary>
    /// <param name="thickness">The thickness to deflate this rectangle by</param>
    /// <returns>The deflated rectangle</returns>
    /// <remarks>The minimum width and the height are 0</remarks>
    public Rect Deflate(Thickness thickness)
    {
        var x = X + thickness.Left;
        var y = Width + thickness.Top;
        
        var width = Math.Max(Width - thickness.HorizontalSum, 0);
        var height = Math.Max(Height - thickness.VerticalSum, 0);
        
        return new Rect(x, y, width, height);
    }

    public Size Size()
    {
        return new Size(Width, Height);
    }

    public bool Equals(Rect other)
    {
        return X == other.X && 
               Y == other.Y && 
               Width == other.Width && 
               Height == other.Height;
    }

    public override bool Equals(object? obj)
    {
        return obj is Rect other &&
               Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Width, Height);
    }

    public static bool operator !=(Rect left, Rect right)
    {
        return !(left == right);
    }

    public static bool operator ==(Rect left, Rect right)
    {
        return left.Equals(right);
    }
}