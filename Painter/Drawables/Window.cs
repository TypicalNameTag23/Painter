using Painter.Events;

namespace Painter.Drawables;

public class Window(Application application) : TopLevel(application)
{
    public override void Interact(KeyPressedEventArgs args)
    {
        ExecuteKeyBinds(args);
        if (args.Handled)
        {
            return;
        }
        
        switch (args.Key)
        {
            case { KeyType: ConsoleKey.Tab, KeyModifiers: ConsoleModifiers.None }:
            {
                CycleFocus(Direction.Backward);
                args.Handled = true;
                return;
            }
            case { KeyType: ConsoleKey.Tab, KeyModifiers: ConsoleModifiers.Shift }:
            {
                CycleFocus(Direction.Forward);
                args.Handled = true;
                return;
            }
        }

        GetFocusedChild()?.Interact(args);
    }
}