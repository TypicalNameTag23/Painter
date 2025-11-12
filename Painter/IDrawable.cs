using Painter.Drawing;

namespace Painter;

public interface IDrawable
{
    public int? Width { get; set; }
    
    public int? Height { get; set; }
    
    public void Draw(Canvas canvas);
}