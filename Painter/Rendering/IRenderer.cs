using Painter.Drawables;

namespace Painter.Rendering;

public interface IRenderer
{
    public void InvalidateRender(Drawable drawable);
    public void ExecuteRenderPass();
}