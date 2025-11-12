using Painter.Drawing;

namespace Painter.Components;

public class Window : Drawable
{
    public Drawable? Drawable { get; set; }
    
    public override void DrawOnto(Canvas canvas)
    {
        Drawable?.DrawOnto(canvas);
    }
}