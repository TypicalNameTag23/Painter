using Painter.Drawing;
using Painter.Elements;
using Painter.Layouts;

namespace Sample;

public class PanelWrapper : Element
{
    private Panel _p = new Panel();

    public PanelWrapper()
    {
        _p.Parent = this;
        _p.Add(new TextBlock()
        {
            Text = "text 1",
            Height = 1,
            Y = 0
        });
        _p.Add(new TextBlock()
        {
            Text = "text 2",
            Height = 1,
            Y = 1
        });
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        _p.Draw(canvas, force);
    }
}