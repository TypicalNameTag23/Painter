using Painter.Drawing;
using Painter.Drawing.Brushes;
using Painter.Drawing.Canvas;

namespace Painter.Drawables.Control;

public abstract class DrawableControl : Drawable
{
    public IBrush BackgroundColor { get; set; }
    
    public IBrush ForegroundColor { get; set; }

    public Thickness Margin { get; set; }
    
    public override void Draw(ICanvas canvas)
    {
        canvas = canvas.Subsection(X + Margin.Left, Y + Margin.Top, Width - Margin.Right - Margin.Right, Height - Margin.Bottom - Margin.Bottom);
        for (var y = 0; y < canvas.Height; y++)
        {
            for (var x = 0; x < canvas.Width; x++)
            {
                canvas.Draw(x, y, new StrokeStyle
                {
                    BackgroundColor = BackgroundColor.GetColorAt(x, y)
                });
            }
        }
    }
}