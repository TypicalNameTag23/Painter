using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Painter.Drawing.Brushes;

namespace Painter.Drawing;

/// <summary>
/// A color
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
    
    #region Red
    
    public static readonly Color IndianRed = new(205, 92, 92); // erm, html kinda anti woke smh
    public static readonly Color LightCoral = new(240, 128, 128);
    public static readonly Color Salmon = new(250, 128, 114);
    public static readonly Color DarkSalmon = new(233, 150, 122);
    public static readonly Color LightSalmon = new(255, 160, 122);
    public static readonly Color Crimson = new(220, 20, 60);
    public static readonly Color FireBrick = new(178, 34, 34);
    public static readonly Color DarkRed = new(139, 0, 0);
    
    #endregion

    #region Pink
    
    public static readonly Color Pink = new(255, 192, 203);
    public static readonly Color LightPink = new(255, 192, 203);
    public static readonly Color HotPink = new(255, 105, 180);
    public static readonly Color DeepPink = new(255, 20, 147);
    public static readonly Color MediumVioletRed = new(199, 21, 133);
    public static readonly Color PaleVioletRed = new(219, 112, 147);
    
    #endregion

    #region Orange
    
    public static readonly Color Coral = new(255, 127, 80);
    public static readonly Color Tomato = new(255, 99, 71);
    public static readonly Color OrangeRed = new(255, 69, 0);
    public static readonly Color DarkOrange = new(255, 140, 0);
    public static readonly Color Orange = new(255, 165, 0);

    #endregion

    #region Yellow
    
    public static readonly Color Gold = new(255, 215, 0);
    public static readonly Color Yellow = new(255, 255, 0);
    public static readonly Color LightYellow = new(255, 255, 224);
    public static readonly Color LemonChiffon = new(255, 250, 205);
    public static readonly Color LightGoldenrodYellow = new(250, 250, 210);
    public static readonly Color PapayaWhip = new(255, 239, 213);
    public static readonly Color Moccasin = new(255, 228, 181);
    public static readonly Color PeachPuff = new(255, 218, 185);
    public static readonly Color PaleGoldenrod = new(238, 232, 170);
    public static readonly Color Khaki = new(240, 230, 140);
    public static readonly Color DarkKhaki = new(189, 183, 107);

    #endregion

    #region Purple
    
    public static readonly Color Lavender = new(	230, 230, 250);
    public static readonly Color Thistle = new(	216, 191, 216);
    public static readonly Color Plum = new(221, 160, 221);
    public static readonly Color Violet = new(	238, 130, 238);
    public static readonly Color Orchid = new(218, 112, 214);
    public static readonly Color Fuchsia = new(255, 0, 255);
    public static readonly Color Magenta = new(255, 0, 255);
    public static readonly Color MediumOrchid = new(186, 85, 211);
    public static readonly Color MediumPurple = new(	147, 112, 219);
    public static readonly Color RebeccaPurple = new(102, 51, 153);
    public static readonly Color BlueViolet = new(138, 43, 226);
    public static readonly Color DarkViolet = new(148, 0, 211);
    public static readonly Color DarkOrchid = new(153, 50, 204);
    public static readonly Color DarkMagenta = new(139, 0, 139);
    public static readonly Color Purple = new(128, 0, 128);
    public static readonly Color Indigo = new(75, 0, 130);
    public static readonly Color SlateBlue = new(106, 90, 205);
    public static readonly Color DarkSlateBlue = new(72, 61, 139);
    public static readonly Color MediumSlateBlue = new(	123, 104, 238);

    #endregion

    #region Green
    
    public static readonly Color GreenYellow = new(173, 255, 47);
    public static readonly Color Chartreuse = new(127, 255, 0);
    public static readonly Color LawnGreen = new(124, 252, 0);
    public static readonly Color Lime = new(0, 255, 0);
    public static readonly Color LimeGreen = new(50, 205, 50);
    public static readonly Color PaleGreen = new(152, 251, 152);
    public static readonly Color LightGreen = new(144, 238, 144);
    public static readonly Color MediumSpringGreen = new(0, 250, 154);
    public static readonly Color SpringGreen = new(0, 255, 127);
    public static readonly Color MediumSeaGreen = new(60, 179, 113);
    public static readonly Color SeaGreen = new(46, 139, 87);
    public static readonly Color ForestGreen = new(34, 139, 34);
    
    /// <summary>
    /// HTML Green color
    /// </summary>
    public static readonly Color LeafGreen = new(0, 128, 0);
    public static readonly Color DarkGreen = new(0, 100, 0);
    public static readonly Color YellowGreen = new(154, 205, 50);
    public static readonly Color OliveDrab = new(	107, 142, 35);
    public static readonly Color Olive = new(128, 128, 0);
    public static readonly Color DarkOliveGreen = new(85, 107, 47);
    public static readonly Color MediumAquamarine = new(102, 205, 170);
    public static readonly Color DarkSeaGreen = new(143, 188, 139);
    public static readonly Color LightSeaGreen = new(32, 178, 170);
    public static readonly Color DarkCyan = new(0, 139, 139);
    public static readonly Color Teal = new(0, 128, 128);

    #endregion
    
    #endregion
}