namespace Painter.Drawing.Canvas;

public abstract class Canvas : ICanvas
{
    public int Width { get; set; }
    
    public int Height { get; set; }

    public abstract void Draw(int x, int y, char character, Brush brush);

    public abstract ICanvas Subsection(int x, int y, int width, int height);

    public void Draw(int x, int y, Brush brush)
    {
        Draw(x, y, ' ', brush);
    }

    public void Draw(int x, int y, string content, Brush brush)
    {
        content = content.Replace(Environment.NewLine, "");
        for (var i = 0; i < content.Length; i++)
        {
            Draw(x + i, y, content[i], brush);
        }
    }

    public void Draw(int x, int y, object obj, Brush brush)
    {
        var text = obj.ToString();
        if (text is not null)
        {
            Draw(x, y, text, brush);
        }
    }
}