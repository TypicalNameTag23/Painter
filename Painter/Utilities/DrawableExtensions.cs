using Painter.Drawables;

namespace Painter.Utilities;

public static class DrawableExtensions
{
    public static TopLevel? GetTopLevel(this Drawable drawable)
    {
        var ancestor = drawable.Parent;
        while (ancestor is not null && ancestor is not TopLevel)
        {
            ancestor = ancestor.Parent;
        }

        return (TopLevel?) ancestor;
    }
}