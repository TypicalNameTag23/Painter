using Painter.Drawing;
using Painter.Elements;

namespace Painter;

public class MainWindow : Window
{
    public ApplicationContext ApplicationContext { get; }

    public MainWindow(ApplicationContext context)
    {
        ApplicationContext = context;
        Width = ApplicationContext.Width;
        Height = ApplicationContext.Height;
    }
    
    public override IDrawable? Parent
    {
        get => null;
        set => throw new InvalidOperationException("Unable to set parent drawable on MainWindow class.");
    }
    
    public void RequestRedraw(Element element)
    {
        ApplicationContext.NeedsPartialRedraw = true;
    }
}