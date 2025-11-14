using Painter.Drawing;
using Painter.Elements;

namespace Painter;

public class MainWindow : Window
{
    public Application Application { private get; init; }
    
    public override IDrawable? Parent
    {
        get => null;
        set => throw new InvalidOperationException("Unable to set parent drawable on MainWindow class.");
    }
    
    public void RequestRedraw(Element element)
    {
        Application.RequestRedraw(element);
    }
}