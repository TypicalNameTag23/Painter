using Painter.Drawing;
using Painter.Drawing.Canvas;

namespace Painter.Drawables;

public abstract partial class Drawable
{
    internal readonly List<Drawable> _children = [];

    /// <summary>
    /// Returns a <see cref="IReadOnlyList"/> of the current <see cref="Drawables.Drawable"/>s that are attached to this <see cref="Drawables.Drawable"/>
    /// </summary>
    protected IReadOnlyList<Drawable> Children => _children.AsReadOnly();

    /// <summary>
    /// Attaches a child <see cref="index"/> to this drawable
    /// </summary>
    /// <param name="index">The index to attach this drawable at</param>
    /// <param name="drawable">The drawable to attach</param>
    /// <remarks>
    /// The rendering of children starts at the lowest index, meaning that the child at index 0 will likely get drawn over if there is more than one child
    /// </remarks>
    protected void AttachAt(int index, Drawable drawable)
    {
        ArgumentNullException.ThrowIfNull(drawable);
        ArgumentOutOfRangeException.ThrowIfLessThan(0, Children.Count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, Children.Count);
        if (drawable.Parent is not null)
        {
            throw new ArgumentException("Drawable is already attached to a parent");
        }
        
        drawable.Parent = this;
        drawable.Root = Root;
        _children.Insert(index, drawable);
        InvalidateDraw();
        // NeedsRedraw = true;
    }
    
    /// <summary>
    /// Attaches a drawable to this drawable
    /// </summary>
    /// <param name="drawable">The drawable to attach</param>
    protected void Attach(Drawable drawable)
    {
        AttachAt(_children.Count, drawable);
    }
    
    /// <summary>
    /// Attaches a drawable to this drawable
    /// </summary>
    /// <param name="drawable">The drawable to attach</param>
    protected void AttachFirst(Drawable drawable)
    {
        AttachAt(0, drawable);
    }

    /// <summary>
    /// Detaches a drawable from this drawable
    /// </summary>
    /// <param name="drawable"></param>
    protected void Detach(Drawable drawable)
    {
        if (drawable.Parent is null)
        {
            return;
        }
        
        drawable.Parent = null;
        drawable.Root = null;
        _children.Remove(drawable);
        InvalidateDraw();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    protected void Detach(int index)
    {
        ArgumentNullException.ThrowIfNull(index);
        if (index < 0 || index >= _children.Count)
        {
            return;
        }

        Detach(_children[index]);
    }

    protected void DetachAll()
    {
        while (_children.Count > 0)
        {
            Detach(_children[0]);
        }
    }

    protected List<Drawable> GetFocusableChildren()
    {
        var focusableChildren = new List<Drawable>();
        foreach (var drawable in _children)
        {
            if (drawable.Focusable)
            {
                focusableChildren.Add(drawable);
            }
            
            focusableChildren.AddRange(drawable.GetFocusableChildren());
        }

        focusableChildren.Sort();
        return focusableChildren;
    }

    protected void DrawChildren(ICanvas canvas)
    {
        var canvasSubsection = canvas.Subsection(X, Y, DesiredSize.Width, DesiredSize.Height);
        foreach (var drawable in _children)
        {
            drawable.Draw(canvasSubsection);
        }
    }
}