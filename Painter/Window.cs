using Painter.Drawing;

namespace Painter;

public class Window : IDrawable
{
    public IDrawable? Drawable
    {
        get;
        set
        {
            field?.Parent = null;
            field = value;
            field?.Parent = this;
        }
    }

    public IDrawable? Parent { get; set; }
    
    public int? Width { get; set; }
    
    public int? Height { get; set; }

    public void Draw(Canvas canvas)
    {
        Drawable?.Draw(canvas);
    }
}