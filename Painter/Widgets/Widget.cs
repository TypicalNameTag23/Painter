using Painter.Drawing;
using Painter.Events;

namespace Painter.Widgets;

public abstract class Widget : IDrawable
{
    public event WidgetDirtyEventHandler? OnWidgetMarkedDirty;
    
    public IDrawable? Parent { get; set; }

    public int X { get; set; } = 0;

    public int Y { get; set; } = 0;

    public int? MaxWidth { get; set; }
    
    public int MinWidth
    {
        get;
        set
        {
            if (value < 0)
            {
                field = 0;
                return;
            }

            field = value;
        }
    }

    public int? Width
    {
        get => field ?? Parent?.Width;
        set
        {
            if (value < MinWidth)
            {
                field = MinWidth;
                return;
            }

            if (value > MaxWidth)
            {
                field = MaxWidth;
                return;
            }

            field = value;
        }
    }

    public int? MaxHeight { get; set; }

    public int MinHeight
    {
        get;
        set
        {
            if (value < 0)
            {
                field = 0;
                return;
            }

            field = value;
        }
    }

    public int? Height 
    {
        get => field ?? Parent?.Height;
        set
        {
            if (value < MinHeight)
            {
                field = MinHeight;
                return;
            }

            if (value > MaxWidth)
            {
                field = MaxWidth;
                return;
            }

            field = value;
        }
    }


    public void MarkDirty()
    {
        OnWidgetMarkedDirty?.Invoke(this);
    }
    
    public abstract void Draw(Canvas canvas);
}