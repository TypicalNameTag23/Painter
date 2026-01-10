using System.Drawing;
using System.Net.Http.Headers;

namespace MusicPlayer.Utilities;

public static class ColorHelper
{
    /// <summary>
    /// Gets a <see cref="Color"/> from an HSV value
    /// </summary>
    /// <param name="hue">The hue value, between 0 (inclusive) and 360 (exclusive)</param>
    /// <param name="saturation">The saturation value, between 0 and 1</param>
    /// <param name="value">The value, between 0 and 1</param>
    /// <returns></returns>
    public static Color FromHsv(double hue, double saturation, double value)
    {
        if (hue is < 0 or >= 360)
        {
            throw new ArgumentOutOfRangeException(nameof(hue), hue, "Hue should be greater than 0 and less than 360");
        }
        
        if (saturation is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(saturation), saturation, "Saturation should be between 0 and 1");
        }
        
        if (value is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "Value should be between 0 and 1");
        }
        
        var chroma = value * saturation;
        hue /= 60;
        var x = chroma * (1 - Math.Abs(hue % 2 - 1));
        var m = value - chroma;

        var chromaPrime = (int) ((chroma + m) * 255);
        var xPrime = (int) ((x + m) * 255);
        var mPrime = (int) (m * 255);
        return hue switch
        {
            >= 0 and < 1 => Color.FromArgb(chromaPrime, xPrime, mPrime),
            >= 1 and < 2 => Color.FromArgb(xPrime, chromaPrime, mPrime),
            >= 2 and < 3 => Color.FromArgb(mPrime, chromaPrime, xPrime),
            >= 3 and < 4 => Color.FromArgb(mPrime, xPrime, chromaPrime),
            >= 4 and < 5 => Color.FromArgb(xPrime, mPrime, chromaPrime),
            _ => Color.FromArgb(chromaPrime, mPrime, xPrime)
        };
    }
}