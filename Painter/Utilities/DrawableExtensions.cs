using Painter.Drawables;

namespace Painter.Utilities;

public static class DrawableExtensions
{
    public static int EffectiveMaxWidth(this Drawable drawable)
    {
        return drawable.Width ?? drawable.MaxWidth;
    }
    
    public static int EffectiveMinWidth(this Drawable drawable)
    {
        return drawable.Width ?? drawable.MinWidth;
    }
    
    public static int EffectiveMaxHeight(this Drawable drawable)
    {
        return drawable.Height ?? drawable.MaxHeight;
    }

    public static int EffectiveMinHeight(this Drawable drawable)
    {
        return drawable.Height ?? drawable.MinHeight;
    }
}