using Painter.Drawing;
using Painter.Elements;

namespace Sample;

public class HsvColorSpaceVisualizer : Element
{
    private int _hueOffset = 0;
    private double _saturationOffset = 0;
    private Timer _timer;
    
    public HsvColorSpaceVisualizer()
    {
        _timer = new Timer(_ =>
        {
            _hueOffset += 1;
            // _saturationOffset += 0.01;
            if (_hueOffset > 359)
            {
                _hueOffset = 0;
            }
            
            // if (_saturationOffset > 1)
            // {
                // _saturationOffset = 0;
            // }
            
            Redraw();
        }, null , 250, 50);
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        var width = Width;
        var height = Height;
        
        var hueStep = 359d / width;
        var saturationStep = 1d / height;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var hue = hueStep * x + _hueOffset;
                if (hue > 359)
                {
                    hue -= 359;
                }
                
                var saturation = saturationStep * y + _saturationOffset;
                if (saturation > 1)
                {
                    saturation -= 1;
                }
                
                canvas.Draw(x, y, " ", new Style
                {
                    BackgroundColor = ColorHelper.FromHsv(hue, saturation, 1)
                });
            }
        }
    }
}