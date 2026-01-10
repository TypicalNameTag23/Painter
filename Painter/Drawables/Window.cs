using Painter.Drawables;
using Painter.Drawing;
using Painter.Elements;

namespace Painter;

public class Window : TopLevel, IDrawable
{
    public Element? Drawable
    {
        get;
        set
        {
            field?.Parent = null;
            field = value;
            field?.Parent = this;
        }
    }

    public virtual IDrawable? Parent { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }

    public void Draw(Canvas canvas, bool force)
    {
        Drawable?.Draw(canvas, force);
    }
    
}