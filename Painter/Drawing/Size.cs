using Painter.Utilities;

namespace Painter.Drawing;

/// <summary>
/// 
/// </summary>
/// <param name="width"></param>
/// <param name="height"></param>
public readonly struct Size(int width, int height) : IEquatable<Size>
{
    public static Size Zero = new(0, 0);
    
    public int Width { get; } = width;

    public int Height { get; } = height;

    /// <summary>
    /// Deflates a size by a certain <see cref="Thickness"/>
    /// </summary>
    /// <param name="thickness">The thickness to deflate this size by</param>
    /// <returns>The deflated size</returns>
    /// <remarks>The minimum width and the height are 0</remarks>
    public Size Deflate(Thickness thickness)
    {
        var width = Math.Max(0, Width - thickness.HorizontalSum);
        var height = Math.Max(0, Height - thickness.VerticalSum);
        
        return new Size(width, height);
    }

    /// <summary>
    /// Deflates a size by a certain <see cref="Size"/>
    /// </summary>
    /// <param name="size">The size to deflate this size by</param>
    /// <returns>The deflated size</returns>
    /// <remarks>The minimum width and the height are 0</remarks>
    public Size Deflate(Size size)
    {
        var width = Math.Max(0, Width - size.Width);
        var height = Math.Max(0, Height - size.Height);
        
        return new Size(width, height);
    }

    /// <summary>
    /// Inflates a size by a certain <see cref="Thickness"/>
    /// </summary>
    /// <param name="thickness">The thickness to inflate this size by</param>
    /// <returns>The inflated size</returns>
    public Size Inflate(Thickness thickness)
    {
        var width = Width + thickness.HorizontalSum;
        var height = Height + thickness.VerticalSum;
        
        return new Size(width, height);
    }

    /// <summary>
    /// Clamps the size to a min and max width and height
    /// </summary>
    /// <param name="minWidth">The minimum width</param>
    /// <param name="maxWidth">The maximum width</param>
    /// <param name="minHeight">The minimum height</param>
    /// <param name="maxHeight">The maximum height</param>
    /// <returns>The clamped size</returns>
    public Size Clamp(int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        var width = Math.Clamp(Width, minWidth, maxWidth);
        var height = Math.Clamp(Height, minHeight, maxHeight);

        return new Size(width, height);
    }

    /// <summary>
    /// Clamps this size to another if that size is smaller
    /// </summary>
    /// <param name="other">The size</param>
    /// <returns>The clamped size</returns>
    public Size Clamp(Size other)
    {
        if (other.Width > Width && other.Height > Height)
        {
            return this;
        }

        if (other.Width < Width && other.Height < Height)
        {
            return other;
        }
        
        var width = Math.Min(Width, other.Width);
        var height = Math.Min(Height, other.Height);
        
        return new Size(width, height);
    }

    /// <summary>
    /// Constrains this size to another one if it is smaller than it
    /// </summary>
    /// <param name="other">The other size</param>
    /// <returns>The constrained size</returns>
    public Size Constrain(Size other)
    {
        if (other.Width > Width && other.Height > Height)
        {
            return this;
        }

        if (other.Width < Width && other.Height < Height)
        {
            return other;
        }

        return new Size(Math.Min(other.Width, Width), Math.Min(other.Height, Height));
    }

    public bool Equals(Size other)
    {
        return Width == other.Width && 
               Height == other.Height;
    }

    public override bool Equals(object? obj)
    {
        return obj is Size other && 
               Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height);
    }

    public static bool operator ==(Size right, Size left)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Size right, Size left)
    {
        return !(right == left);
    }
}