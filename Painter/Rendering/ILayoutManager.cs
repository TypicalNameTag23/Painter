using Painter.Drawables;

namespace Painter.Rendering;

public interface ILayoutManager
{
    public void InvalidateArrange(Drawable drawable);
    public void InvalidateMeasure(Drawable drawable);
    public void ExecuteLayoutPass();
}