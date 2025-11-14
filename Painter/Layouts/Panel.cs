using Painter.Drawing;
using Painter.Elements;

namespace Painter.Layouts;

public class Panel : Element
{
    private readonly List<Element> _elements = [];

    public override bool NeedsRedraw => true;

    public void Add(Element element)
    {
        _elements.Add(element);
        element.Parent = this;
        Redraw();
    }
    
    public void Add(Element[] elements)
    {
        foreach (var element in elements)
        {
            _elements.Add(element);
            element.Parent = this;
        }
        
        Redraw();
    }

    public void Remove(Element element)
    {
        _elements.Remove(element);
        element.Parent = null;
        Redraw();
    }

    public void RemoveAll()
    {
        _elements.Clear();
        Redraw();
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        for (var i = _elements.Count - 1; i >= 0; i--)
        {
            _elements[i].Draw(canvas, true); // todo: way to detect when elements intersect with each other and force redraw to retain ordering 
        }
    }
    
}