using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Painter.Drawing.Brushes;

namespace Painter.Drawing;

/// <summary>
/// 
/// </summary>
public readonly record struct Color(byte R, byte G, byte B) 
{
    public static Color FromHex(string hex)
    {
        if (hex.Length != 6)
        {
            if (hex is not ['#', _, _, _, _, _, _])
            {
                throw new FormatException("A hex string should be either 6 characters long, or 7 including the #");
            }

            hex = hex[1..];
        }

        return new Color(
            byte.Parse(hex[..2], NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            byte.Parse(hex[2..4], NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            byte.Parse(hex[4..6], NumberStyles.HexNumber, CultureInfo.InvariantCulture));
    }
    
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

        var chromaPrime = (byte) ((chroma + m) * 255);
        var xPrime = (byte) ((x + m) * 255);
        var mPrime = (byte) (m * 255);
        return hue switch
        {
            >= 0 and < 1 => new Color(chromaPrime, xPrime,      mPrime),
            >= 1 and < 2 => new Color(xPrime,      chromaPrime, mPrime),
            >= 2 and < 3 => new Color(mPrime,      chromaPrime, xPrime),
            >= 3 and < 4 => new Color(mPrime,      xPrime,      chromaPrime),
            >= 4 and < 5 => new Color(xPrime,      mPrime,      chromaPrime),
            _ =>            new Color(chromaPrime, mPrime,      xPrime)
        };
    }
    
    /// <summary>
    /// Special transparent color
    /// </summary>
    /// <remarks>This is a null value and unable to be blended between. The only <see cref="IBrush"/> that this color can be used in is a <see cref="SolidColorBrush"/></remarks>
    public static readonly Color? Transparent = null;

    #region Absolute Colors
    
    public static readonly Color Red = new(255, 0, 0);
    public static readonly Color Green = new(0, 255, 0);
    public static readonly Color Blue = new(0, 0, 255);
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(255, 255, 255);
    
    #endregion
    
    #region HTML Named Colors 
    // Taken from https://htmlcolorcodes.com/color-names/
    
    #region Reds
    
    public static readonly Color IndianRed = new(205, 92, 92); // erm, html kinda anti woke smh
    public static readonly Color LightCoral = new(240, 128, 128);
    public static readonly Color Salmon = new(250, 128, 114);
    public static readonly Color DarkSalmon = new(233, 150, 122);
    public static readonly Color LightSalmon = new(255, 160, 122);
    public static readonly Color Crimson = new(220, 20, 60);
    public static readonly Color FireBrick = new(178, 34, 34);
    public static readonly Color DarkRed = new(139, 0, 0);
    
    #endregion

    #region Pinks
    
    public static readonly Color Pink = new(255, 192, 203);
    public static readonly Color LightPink = new(255, 192, 203);
    public static readonly Color HotPink = new(255, 105, 180);
    public static readonly Color DeepPink = new(255, 20, 147);
    public static readonly Color MediumVioletRed = new(199, 21, 133);
    public static readonly Color PaleVioletRed = new(219, 112, 147);
    
    #endregion
    
    #endregion
}