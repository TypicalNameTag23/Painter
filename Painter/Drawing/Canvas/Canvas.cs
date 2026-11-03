namespace Painter.Drawing.Canvas;

public abstract class Canvas : ICanvas
{
    public int Width { get; set; }
    
    public int Height { get; set; }

    public abstract void Draw(int x, int y, char character, StrokeStyle strokeStyle);

    public void Draw(int x, int y, StrokeStyle strokeStyle)
    {
        Draw(x, y, ' ', strokeStyle);
    }

    public void Draw(int x, int y, string content, StrokeStyle strokeStyle)
    {
        content = content.Replace(Environment.NewLine, "");
        for (var i = 0; i < content.Length; i++)
        {
            Draw(x + i, y, content[i], strokeStyle);
        }
    }

    public void Draw(int x, int y, object obj, StrokeStyle strokeStyle)
    {
        var text = obj.ToString();
        if (text is not null)
        {
            Draw(x, y, text, strokeStyle);
        }
    }

    public virtual ICanvas Subsection(int x, int y, int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(width, Width);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(height, Height);
        
        return new SubsectionCanvas(x, y, width, height, this);
    }
}