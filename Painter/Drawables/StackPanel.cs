using Painter.Drawing;

namespace Painter.Drawables;

public class StackPanel : Panel
{
    public Orientation Orientation
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = Orientation.Vertical;

    public override void DrawImpl(ICanvas canvas)
    {
        switch (Orientation)
        {
            case Orientation.Horizontal:
            {
                DrawChildrenHorizontally(canvas);
                break;
            }
            case Orientation.Vertical:
            {
                DrawChildrenVertically(canvas);
                break;
            }
        }
    }

    private void DrawChildrenVertically(ICanvas canvas)
    {
        var yOffset = 0;
        foreach (var drawable in _children)
        {
            drawable.DrawImpl(canvas.Subsection(X, Y + yOffset, Width, Height));
            yOffset += drawable.Height;
        }
    }

    private void DrawChildrenHorizontally(ICanvas canvas)
    {
        var xOffset = 0;
        foreach (var drawable in _children)
        {
            drawable.DrawImpl(canvas.Subsection(X + xOffset, Y, Width, Height));
            xOffset += drawable.Width;
        }
    }
}