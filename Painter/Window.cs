using Painter.Drawing;

namespace Painter.Components;

public class Window : IDrawable
{
    public IDrawable? Drawable { get; set; }

    public int? Width { get; set; }
    
    public int? Height { get; set; }

    public void Draw(Canvas canvas)
    {
        Drawable?.Draw(canvas);
    }
}