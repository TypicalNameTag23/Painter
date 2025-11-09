using Taui.Drawing;

namespace Taui.Composeables;

public class Window : IComposable
{
    public IComposable? Composable { get; set; }

    public void ComposeOnto(Canvas canvas)
    {
        Composable?.ComposeOnto(canvas);
    }
}