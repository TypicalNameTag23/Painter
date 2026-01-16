using Painter.Drawables;
using Painter.Drawing;

namespace Painter.Utilities;

public static class SizeExtensions
{
    /// <summary>
    /// Clamps the size to the drawable's effective minimum and maximum height and width
    /// </summary>
    /// <param name="size">The size</param>
    /// <param name="drawable">The drawable</param>
    /// <returns>The clamped size</returns>
    public static Size Clamp(this Size size, Drawable drawable)
    {
        return size.Clamp(
            drawable.EffectiveMinWidth(), 
            drawable.EffectiveMaxWidth(), 
            drawable.EffectiveMinHeight(), 
            drawable.EffectiveMaxHeight());
    }
}