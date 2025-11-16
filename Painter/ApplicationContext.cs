using Painter.Drawing;
using Painter.Platform;

namespace Painter;

public class ApplicationContext
{
    public bool NeedsPartialRedraw { get; set; } = false;
    public bool NeedsFullRedraw { get; set; } = true;
    
    public Terminal Terminal { get; } = Terminal.GetPlatformTerminal();
    public int Width => Terminal.Width;
    public int Height => Terminal.Height;

    public ApplicationState ApplicationState
    {
        get;
        set
        {
            if (field != ApplicationState.Inactive && value == ApplicationState.Inactive)
            {
                return;
            }

            field = value;
        }
    } = ApplicationState.Inactive;
    
}