using Painter.Drawing;

namespace Painter.Components;

public abstract class Drawable
{
    private Drawable? _parent;
    
    public abstract void DrawOnto(Canvas canvas);

    public bool IsAttached()
    {
        return _parent != null && _parent.IsAttached();
    }

    public void Attach(Drawable drawable)
    {
        _parent = drawable;
    }
    
    public void Detach()
    {
        _parent = null;
    }
}