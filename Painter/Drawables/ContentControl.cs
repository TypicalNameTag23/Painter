using Painter.Drawables.DrawableImpl;
using Painter.Drawing;
using Painter.Events;

namespace Painter.Drawables;

public class ContentControl : Drawable
{
    public Drawable? Content
    {
        get;
        set
        {
            if (value == field)
            {
                return;
            }

            if (field is not null)
            {
                Detach(_children[0]);                
            }

            field = value;

            if (field is not null)
            {
                Attach(field);
            }
        }
    }
}