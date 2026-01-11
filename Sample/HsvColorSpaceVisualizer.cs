using System;
using System.Threading;
using Painter.Drawables.DrawableImpl;
using Painter.Drawing;
using Painter.Events;

namespace Sample;

public class HsvColorSpaceVisualizer : Drawable
{
    private readonly Timer _timer;

    public HsvColorSpaceVisualizer()
    {
        _timer = new Timer(_ =>
        {
            HueOffset += 1;
            if (HueOffset > 359)
            {
                HueOffset = 0;
            }
        }, null , 250, 50);
    }

    public double HueOffset
    {
        get;
        set
        {
            var offset = value;
            if (offset is > 359 or < 0)
            {
                offset = 0;
            }

            SetAndRedrawIfChanged(ref field, offset);
        }
    }

    public double SaturationOffset
    {
        get;
        set
        {
            var offset = value;
            if (offset is > 1 or < 0)
            {
                offset = 0;
            }

            SetAndRedrawIfChanged(ref field, offset);
        }
    }

    public double Value
    {
        get;
        set
        {
            var offset = value;
            if (offset > 1)
            {
                offset = 1;
            }
            else if (offset < 0)
            {
                offset = 0;
            }

            SetAndRedrawIfChanged(ref field, offset);
        }
    } = 1;

    public override bool Focusable { get; set; } = true;

    public override bool Focused
    {
        get;
        set
        {
            SetAndRedrawIfChanged(ref field, value);
            Value = Focused ? 0.7 : 1;
        }
    }

    protected override void Draw(ICanvas canvas)
    {
        var width = Width;
        var height = Height;
        
        var hueStep = 359d / width;
        var saturationStep = 1d / height;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var hue = hueStep * x + HueOffset;
                if (hue > 359)
                {
                    hue -= 359;
                }
                
                var saturation = saturationStep * y + SaturationOffset;
                if (saturation > 1)
                {
                    saturation -= 1;
                }
                
                canvas.Draw(x, y, " ", new Style
                {
                    BackgroundColor = ColorHelper.FromHsv(hue, saturation, Value)
                });
            }
        }
    }

    public override void Interact(KeyPressedEventArgs args)
    {
        if (args.Key.KeyType != ConsoleKey.Spacebar)
        {
            return;
        }
        
        args.Handled = true;
        SaturationOffset += 0.1;
        RequestRedraw();
    }
}