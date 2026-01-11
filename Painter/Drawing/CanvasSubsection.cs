namespace Painter.Drawing;

public class CanvasSubsection : ICanvas
{
    private readonly ICanvas _fullCanvas;

    internal CanvasSubsection(int xOffset, int yOffset, int width, int height, ICanvas fullCanvas)
    {
        XOffset = xOffset;
        YOffset = yOffset;
        Width = width;
        Height = height;
        _fullCanvas = fullCanvas;
    }

    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public int XOffset { get; set; }
    
    public int YOffset { get; set; }

    public void Draw(int x, int y, char character, Style style)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            return;
        }
        
        _fullCanvas.Draw(x + XOffset, y + YOffset, character, style);
    }

    public void Draw(int x, int y, Style style)
    {
        Draw(x + XOffset, y + YOffset, ' ', style);
    }

    public void Draw(int x, int y, object obj, Style style)
    {
        var text = obj.ToString();
        text = text?.Replace(Environment.NewLine, "");
        for (var i = 0; i < text?.Length; i++)
        {
            Draw(x + i, y, text[i], style);
        }
    }

    public ICanvas Subsection(int x, int y, int width, int height)
    {
        return new CanvasSubsection(x + XOffset, y + YOffset, width, height, _fullCanvas);
    }
}