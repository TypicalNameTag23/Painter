using Taui.Drawing;

namespace Taui.Composeables;

public class Window : IDrawable
{
    public IDrawable? Drawable { get; set; }

    public void DrawOnto(Canvas canvas)
    {
        Drawable?.DrawOnto(canvas);
    }
}