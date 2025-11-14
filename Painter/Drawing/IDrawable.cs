namespace Painter.Drawing;

public interface IDrawable
{
    public IDrawable? Parent { get; set; }
    
    public int? Width { get; set; }
    
    public int? Height { get; set; }
    
    public void Draw(Canvas canvas, bool force);
}