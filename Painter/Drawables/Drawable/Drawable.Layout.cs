using Painter.Drawing;
using Painter.Utilities;

namespace Painter.Drawables;

public partial class Drawable
{
    private Size? _previousMeasureSize;
    private Rect? _previousArrangeSize;

    /// <summary>
    /// Gets the X position of this <see cref="Drawable"/> on the screen
    /// </summary>
    public int X
    {
        get; 
        set => SetAndRedrawIfChanged(ref field, value);
    } = 0;

    /// <summary>
    /// Gets the Y position of this <see cref="Drawable"/> on the screen
    /// </summary>
    public int Y
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = 0;

    /// <summary>
    /// Gets the maximum width
    /// </summary>
    public int MaxWidth
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, MinWidth);
            SetAndRedrawIfChanged(ref field, value);
        }
    }

    /// <summary>
    /// Gets the minimum width
    /// </summary>
    public int MinWidth
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxWidth);
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            SetAndRedrawIfChanged(ref field, value);
        }
    } = 0;

    /// <summary>
    /// Gets the width
    /// </summary>
    public int? Width
    {
        get;
        set
        {
            if (field == value)
            {
                return;
            }
            
            if (value is null)
            {
                field = value;
                return;
            }
            
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Value, MaxWidth);
            ArgumentOutOfRangeException.ThrowIfLessThan(value.Value, MinWidth);
            SetAndRedrawIfChanged(ref field, value);
        }
    }

    public int MaxHeight
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, MinHeight);
            SetAndRedrawIfChanged(ref field, value);
        }
    } = int.MaxValue;

    public int MinHeight
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxHeight);
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            SetAndRedrawIfChanged(ref field, value);
        }
    } = 0;

    public int? Height
    {
        get;
        set
        {
            if (field == value)
            {
                return;
            }
            
            if (value is null)
            {
                field = value;
                return;
            }
            
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Value, MaxHeight);
            ArgumentOutOfRangeException.ThrowIfLessThan(value.Value, MinHeight);
            SetAndRedrawIfChanged(ref field, value);
        }
    }
    
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;
    
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;

    public Thickness Margin { get; set; }
    
    public bool IsMeasureValid { get; private set; }

    public bool IsArrangeValid { get; private set; }
    
    public Size DesiredSize { get; set; }

    public void InvalidateMeasure()
    {
        if (!IsMeasureValid)
        {
            return;
        }
        
        Root?.LayoutManager.InvalidateMeasure(this);
        IsMeasureValid = false;
    }

    public void InvalidateArrange()
    {
        if (!IsArrangeValid)
        {
            return;
        }
        
        Root?.LayoutManager.InvalidateArrange(this);
        IsArrangeValid = false;
    }
    
    /// <summary>
    /// Measures the desired size of this drawable
    /// </summary>
    /// <param name="availableSize">The size available to this drawable</param>
    public virtual void Measure(Size availableSize)
    {
        if ((IsMeasureValid && _previousMeasureSize == availableSize) || !IsVisible)
        {
            return;
        }
        
        IsMeasureValid = true;
        var size = availableSize
            .Deflate(new Size(X, Y)) // Subtract the X and Y offset from the available area
            .Deflate(Margin)                    // Subtract the margin from the available area
            .Clamp(this);                       // Clamp to the min and max with and height of this drawable
        
        DesiredSize = MeasureImpl(size)                                         // Measure using the drawable's implementation
            .Inflate(Margin)                                                    // Add the margin back
            .Clamp(0, availableSize.Width, 0, availableSize.Height);   // Clamp between 0 and the max width and height available
        _previousMeasureSize = availableSize;
    }

    public virtual void Arrange(Rect bounds)
    {
        if (IsArrangeValid && _previousArrangeSize == bounds)
        {
            return;
        }

        var boundsSize = bounds.Size();
        if (!IsMeasureValid && _previousMeasureSize != boundsSize)
        {
            Measure(bounds.Size());
        }
        
        IsArrangeValid = true;
        var originX = bounds.X + Margin.Left;
        var originY = bounds.Y + Margin.Top;
        var sizeWithoutMargins = boundsSize.Deflate(Margin);
        
        var size = sizeWithoutMargins
            .Constrain(DesiredSize.Deflate(Margin))
            .Clamp(this);

        size = ArrangeImpl(size).Clamp(size);
        originX += HorizontalAlignment switch
        {
            HorizontalAlignment.Stretch or HorizontalAlignment.Center =>
                (sizeWithoutMargins.Width - size.Width) / 2,
            HorizontalAlignment.Right =>
                sizeWithoutMargins.Width - size.Width,
            _ => 0
        };

        originY += VerticalAlignment switch
        {
            VerticalAlignment.Stretch or VerticalAlignment.Center =>
                (sizeWithoutMargins.Height - size.Height) / 2,
            VerticalAlignment.Bottom =>
                sizeWithoutMargins.Height - size.Height,
            _ => 0
        };
        
        Bounds = new Rect(originX, originY, size.Width, size.Height);
        _previousArrangeSize = bounds;
    }
    
    /// <summary>
    /// The implementation of measure to use when measuring a drawable
    /// </summary>
    /// <param name="availableSize">The size available to this drawable</param>
    /// <returns>The desired size of this drawable</returns>
    protected virtual Size MeasureImpl(Size availableSize)
    {
        var width = 0;
        var height = 0;
        foreach (var child in _children)
        {
            child.Measure(availableSize);
            if (child.DesiredSize.Width > width)
            {
                width = child.DesiredSize.Width;
            }

            if (child.DesiredSize.Height > height)
            {
                height = child.DesiredSize.Height;
            }
        }
        
        return new Size(width, height);
    }

    protected virtual Size ArrangeImpl(Size availableSize)
    {
        var bounds = new Rect(availableSize);
        foreach (var child in _children)
        {
            child.Arrange(bounds);
        }
        
        return availableSize;
    }
}