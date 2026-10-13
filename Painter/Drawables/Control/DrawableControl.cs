using Painter.Drawing;
using Painter.Drawing.Brushes;
using Painter.Drawing.Canvas;

namespace Painter.Drawables.Control;

public abstract class DrawableControl : ContentControl
{
    public IBrush? BackgroundColor { get; set; }
    
    public IBrush? ForegroundColor { get; set; }

    public Thickness Margin { get; set; }
    
    public Thickness Padding { get; set; }
    
    public override void Draw(ICanvas canvas)
    {
        var subsection = canvas.Subsection(
            X + Margin.Left, 
            Y + Margin.Top, 
            Width - (Margin.Right + Margin.Left), 
            Height - (Margin.Bottom + Margin.Top)); // Remove space defined by margin
        
        if (BackgroundColor is not null)
        {
            for (var y = 0; y < subsection.Height; y++) // draw background
            {
                for (var x = 0; x < subsection.Width; x++)
                {
                    subsection.Draw(x, y, new StrokeStyle
                    {
                        BackgroundColor = BackgroundColor.GetColorAt(x, y, subsection.Width, subsection.Height)
                    });
                }
            }
        }
        
        DrawChildren(subsection);
    }
}