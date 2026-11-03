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
    /// Attaches a child <see cref="index"/> to this <see cref="Painter.Drawables.Drawable"/>
    /// </summary>
    /// <param name="index">The index to attach this drawable at</param>
    /// <param name="drawable">The drawable to attach</param>
    /// <remarks>
    /// The <see cref="Drawables.Drawable"/> will not be attached if it is already attached to another <see cref="Drawables.Drawable"/>
    /// <br/>
    /// Lower indexes get rendered first
    /// </remarks>
    protected void AttachAt(int index, Drawable drawable)
    {
        if (index > _children.Count)
        {
            index = _children.Count;
        }
        
        if (drawable.Parent is not null)
        {
            return;
        }
        drawable.Parent = this;
        _children.Insert(index, drawable);
        NeedsRedraw = true;
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
        _children.Remove(drawable);
        NeedsRedraw = true;
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
        var canvasSubsection = canvas.Subsection(X, Y, Width, Height);
        foreach (var drawable in _children)
        {
            drawable.Draw(canvasSubsection);
        }
    }
}