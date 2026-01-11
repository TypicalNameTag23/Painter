using Painter.Drawables;

namespace Painter.Utilities;

public static class DrawableExtensions
{
    /// <summary>
    /// Gets the <see cref="TopLevel"/> associated with a <see cref="Drawable"/>
    /// </summary>
    /// <param name="drawable">The <see cref="Drawable"/></param>
    /// <returns>The <see cref="TopLevel"/></returns>
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