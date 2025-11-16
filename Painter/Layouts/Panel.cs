using Painter.Drawing;
using Painter.Elements;

namespace Painter.Layouts;

public class Panel : Element
{
    public List<Element> Elements
    {
        private get;
        init
        {
            field = value;
            foreach (var element in value)
            {
                element.Parent = this;
            }
        }
    } = [];

    public override bool NeedsRedraw => true;

    public void Add(Element element)
    {
        Elements.Add(element);
        element.Parent = this;
        Redraw();
    }
    
    public void Add(Element[] elements)
    {
        foreach (var element in elements)
        {
            Elements.Add(element);
            element.Parent = this;
        }
        
        Redraw();
    }

    public void Remove(Element element)
    {
        Elements.Remove(element);
        element.Parent = null;
        Redraw();
    }

    public void RemoveAll()
    {
        Elements.Clear();
        Redraw();
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        for (var i = Elements.Count - 1; i >= 0; i--)
        {
            Elements[i].Draw(canvas, true); // todo: way to detect when elements intersect with each other and force redraw to retain ordering 
        }
    }
    
}