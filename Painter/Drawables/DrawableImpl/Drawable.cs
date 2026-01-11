using Painter.Drawing;
using Painter.Events;

namespace Painter.Drawables.DrawableImpl;

public abstract partial class Drawable : IComparable
{
    private int? _width;
    private int? _height;

    public bool Visible { get; set; } = true;

    public Drawable? Parent
    {
        get;
        internal set;
    }

    public bool NeedsRedraw
    {
        get;
        set
        {
            if (field == value)
            {
                return;
            }
            
            field = value;
            foreach (var drawable in Children)
            {
                drawable.NeedsRedraw = field;
            }
            
            if (field)
            {
                Parent?.NeedsRedraw = true;
            }
        }
    }

    public int X
    {
        get; 
        set => SetAndRedrawIfChanged(ref field, value);
    } = 0;

    public int Y
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = 0;

    public int? MaxWidth
    {
        get;
        set
        {
            if (value < MinWidth || value < 0)
            {
                return;
            }
            
            SetAndRedrawIfChanged(ref field, value);
        }
    }

    public int MinWidth
    {
        get;
        set
        {
            if (value > MaxWidth || value < 0)
            {
                return;
            }

            SetAndRedrawIfChanged(ref field, value);
        }
    } = 0;

    public int Width
    {
        get
        {
            var width = _width ?? Parent?.Width - X;
            if (width is null)
            {
                return MinWidth;
            }

            if (MaxWidth is not null && width.Value > MaxWidth.Value)
            {
                return MaxWidth.Value;
            }

            return width.Value < MinWidth ? MinWidth : width.Value;
        }
        set
        {
            var newValue = value;
            if (value < MinWidth)
            {
                newValue = MinWidth;
            }

            if (value > MaxWidth)
            {
                newValue = MaxWidth.Value;
            }

            SetAndRedrawIfChanged(ref _width, newValue);
        }
    }

    public int? MaxHeight
    {
        get; 
        set
        {
            if (value < MinHeight || value < 0)
            {
                return;
            }
            
            SetAndRedrawIfChanged(ref field, value);
        }
    }

    public int MinHeight
    {
        get;
        set
        {
            if (value > MaxHeight || value < 0)
            {
                return;
            }

            SetAndRedrawIfChanged(ref field, value);
        }
    } = 0;

    public int Height 
    {
        get 
        {
            var height = _height ?? Parent?.Height - Y;
            if (height is null)
            {
                return MinHeight;
            }

            if (MaxHeight is not null && height.Value > MaxHeight.Value)
            {
                return MaxHeight.Value;
            }

            return height.Value < MinHeight ? MinHeight : height.Value;
        }
        set
        {
            var newValue = value;
            if (value < MinHeight)
            {
                newValue = MinHeight;
            }

            if (value > MaxWidth)
            {
                newValue = MaxWidth.Value;
            }

            SetAndRedrawIfChanged(ref _height, newValue);
        }
    }

    public void Draw(Canvas canvas)
    {
        DrawImpl(canvas);
        
        foreach (var drawable in _children)
        {
            drawable.Draw(canvas);
        }
    }

    public void RequestRedraw()
    {
        NeedsRedraw = true;
    }

    /// <summary>
    /// Draws this drawable to the canvas
    /// </summary>
    /// <param name="canvas"></param>
    protected virtual void DrawImpl(Canvas canvas) { }

    public int CompareTo(object? obj)
    {
        if (obj is Drawable other)
        {
            return CompareTo(other);
        }

        return -1;
    }

    public int CompareTo(Drawable? other)
    {
        if (other is null || FocusOrder < other.FocusOrder)
        {
            return -1;
        }
            
        return FocusOrder > other.FocusOrder ? 1 : 0;
    }
    
    protected void SetAndRedrawIfChanged<T>(ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        NeedsRedraw = true;
    }
}