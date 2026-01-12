namespace Painter.Drawing.Canvas;

public class CanvasSubsection : Canvas
{
    private readonly ICanvas _fullCanvas;
    private readonly int _xOffset;
    private readonly int _yOffset;

    internal CanvasSubsection(int xOffset, int yOffset, int width, int height, ICanvas fullCanvas)
    {
        _xOffset = xOffset;
        _yOffset = yOffset;
        Width = width;
        Height = height;
        _fullCanvas = fullCanvas;
    }

    public override void Draw(int x, int y, char character, StrokeStyle strokeStyle)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            return;
        }
        
        _fullCanvas.Draw(x + _xOffset, y + _yOffset, character, strokeStyle);
    }

    public override ICanvas Subsection(int x, int y, int width, int height)
    {
        return new CanvasSubsection(
            x + _xOffset, 
            y + _yOffset, 
            width > Width ? Width : width, 
            height > Height ? Height : height, 
            _fullCanvas);
    }
}