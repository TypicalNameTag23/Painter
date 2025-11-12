using Painter.Drawing;
using Painter.Events;

namespace Painter.Widgets;

public abstract class Widget : IDrawable
{
    private IDrawable? _parent;
    private int? _width;
    private int? _height;

    internal event WidgetDirtyEventHandler? OnWidgetDirty;
    
    public IDrawable? Parent
    {
        get => _parent;
        internal set => _parent = value;
    }

    public int X { get; set; } = 0;

    public int Y { get; set; } = 0;

    public int? Width
    {
        get => _width ?? Parent?.Width;
        set => _width = value;
    }
    
    public int? Height 
    {
        get => _height ?? Parent?.Height;
        set => _height = value;
    }
    
    internal void MarkDirty()
    {
        OnWidgetDirty?.Invoke(this);
    }
    
    public abstract void Draw(Canvas canvas);
}