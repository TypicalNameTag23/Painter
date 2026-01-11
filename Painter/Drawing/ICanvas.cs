namespace Painter.Drawing;

public interface ICanvas
{
    public int Width { get; protected set; }
    
    public int Height { get; protected set; }
    
    public void Draw(int x, int y, char character, Style style);
    public void Draw(int x, int y, Style style);
    public void Draw(int x, int y, object obj, Style style);
    public ICanvas Subsection(int x, int y, int width, int height);
}