using Painter.Drawables.DrawableImpl;
using Painter.Drawing;
using Painter.Events;

namespace Painter.Drawables;

public class Window : TopLevel
{
    public override void Interact(KeyPressedEventArgs args)
    {
        switch (args.Key)
        {
            case { KeyType: ConsoleKey.Tab, KeyModifiers: ConsoleModifiers.None }:
            {
                CycleFocus(CycleDirection.Next);
                return;
            }
            case { KeyType: ConsoleKey.Tab, KeyModifiers: ConsoleModifiers.Shift }:
            {
                CycleFocus(CycleDirection.Previous);
                return;
            }
        }

        GetFocusedChild()?.Interact(args);
        if (!args.Handled)
        {
            ExecuteWindowKeyBinds(args.Key);
        }
    }
}