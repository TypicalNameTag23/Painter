using System.Drawing;
using Painter.Drawing;
using Painter.Drawing.Brushes;
using Painter.Drawing.Canvas;

namespace Painter.Drawables;

public abstract partial class Drawable : IComparable
{
    public bool IsVisible { get; set; } = true;
    
    public Rect Bounds { get; private set; }

    public IBrush? BackgroundColor { get; set; }
    
    public bool IsRenderValid { get; set; }
    
    /// <summary>
    /// Gets the drawable's <see cref="TopLevel"/>
    /// </summary>
    public virtual TopLevel? Root { get; private set; }
    
    /// <summary>
    /// Gets the parent drawable
    /// </summary>
    public Drawable? Parent { get; private set; }

    public virtual void Draw(ICanvas canvas)
    {
        if (IsRenderValid)
        {
            return;
        }
        
        DrawImpl(canvas);
        DrawChildren(canvas);
    }
    
    public virtual void DrawImpl(ICanvas canvas) { }

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    protected bool SetAndRedrawIfChanged<T>(ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        InvalidateDraw();
        return true;
    }

    /// <summary>
    /// Invalidates this drawable's draw
    /// </summary>
    protected void InvalidateDraw()
    {
        Root?.Renderer.InvalidateRender(this);
    }
}