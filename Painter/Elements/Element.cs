using Painter.Drawing;

namespace Painter.Elements;

//todo: horizontal and vertical alignments in respect to parent
public abstract class Element : IDrawable
{
    public virtual bool NeedsRedraw { get; private set; }

    public IDrawable? Parent
    {
        get; 
        set => SetAndRedrawIfChanged(ref field, value);
    }

    public int X
    {
        get; 
        set => SetAndRedrawIfChanged(ref field, value);
    } = 0;

    public int Y
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = 0;

    public int? MaxWidth
    {
        get;
        set
        {
            if (value < MinWidth || value < 0)
            {
                return;
            }
            
            SetAndRedrawIfChanged(ref field, value);
        }
    }

    public int MinWidth
    {
        get;
        set
        {
            if (value > MaxWidth || value < 0)
            {
                return;
            }

            SetAndRedrawIfChanged(ref field, value);
        }
    } = 0;

    public int? Width
    {
        get
        {
            var width = field ?? Parent?.Width - X;
            if (width is null)
            {
                return null;
            }

            if (width > MaxWidth)
            {
                return MaxWidth;
            }

            return width < MinWidth ? MinWidth : width;
        }
        set
        {
            var newValue = value;
            if (value < MinWidth)
            {
                newValue = MinWidth;
            }

            if (value > MaxWidth)
            {
                newValue = MaxWidth;
            }

            SetAndRedrawIfChanged(ref field, newValue);
        }
    }

    public int? MaxHeight
    {
        get; 
        set
        {
            if (value < MinHeight || value < 0)
            {
                return;
            }
            
            SetAndRedrawIfChanged(ref field, value);
        }
    }

    public int MinHeight
    {
        get;
        set
        {
            if (value > MaxHeight || value < 0)
            {
                return;
            }

            SetAndRedrawIfChanged(ref field, value);
        }
    } = 0;

    public int? Height 
    {
        get 
        {
            var height = field ?? Parent?.Height - Y;
            if (height is null)
            {
                return null;
            }

            if (height > MaxHeight)
            {
                return MaxHeight;
            }

            return height < MinHeight ? MinHeight : height;
        }
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

    public void Draw(Canvas canvas, bool force)
    {
        if (!NeedsRedraw && !force)
        {
            return;
        }
        
        DrawElement(canvas, force);
        NeedsRedraw = false;
    }

    protected void Redraw()
    {
        NeedsRedraw = true;
        var mainWindow = GetMainWindow();
        mainWindow?.RequestRedraw(this);
    }

    protected MainWindow? GetMainWindow()
    {
        var ancestor = Parent;
        while (ancestor is not null && ancestor is not Window)
        {
            ancestor = ancestor.Parent;
        }

        return (MainWindow?)ancestor;
    }

    protected void SetAndRedrawIfChanged<T>(ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        Redraw();
    }
    
    protected abstract void DrawElement(Canvas canvas, bool force);
}