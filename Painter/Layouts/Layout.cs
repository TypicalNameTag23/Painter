using Painter.Drawing;
using Painter.Widgets;

namespace Painter.Layouts;

public abstract class Layout(int width, int height) : Widget
{
    private List<Widget> _widgets = [];
    
    public abstract override void Draw(Canvas canvas);
}