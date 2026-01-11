using Painter.Drawables.DrawableImpl;
using Painter.Drawing;
using Painter.Events;

namespace Painter.Drawables;

public class Panel : Drawable
{
    public IReadOnlyList<Drawable> Content
    {
        get => Children;
        set
        {
            RemoveAll();
            Add(value);
        }
    }

    public override void Interact(KeyPressedEventArgs args) { }

    public void Add(Drawable drawable)
    {
        AttachFirst(drawable);
    }

    public void Add(params Drawable[] drawables)
    {
        foreach (var drawable in drawables)
        {
            Add(drawable);
        }
    }

    public void Add(IEnumerable<Drawable> drawables)
    {
        foreach (var drawable in drawables)
        {
            Add(drawable);
        }
    }

    public void Remove(Drawable drawable)
    {
        Detach(drawable);
    }

    public void RemoveAll()
    {
        DetachAll();
    }

    protected override void DrawImpl(Canvas canvas) { }
}