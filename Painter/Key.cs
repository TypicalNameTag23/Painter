namespace Painter;

public record Key
{
    public Key(ConsoleKey key, ConsoleModifiers modifier = ConsoleModifiers.None)
    {
        KeyType = key;
        KeyModifiers = modifier;
        // foreach (var modifier in modifiers)
        // {
        //     KeyModifiers |= modifier;
        //     // switch (modifier)
        //     // {
        //     //     case ConsoleModifiers.Alt:
        //     //     {
        //     //         KeyModifiers |= ConsoleModifiers.Alt;
        //     //         break;
        //     //     }
        //     //     case ConsoleModifiers.Shift:
        //     //     {
        //     //         KeyModifiers |= ConsoleModifiers.Shift;
        //     //         break;
        //     //     }
        //     //     case ConsoleModifiers.Control:
        //     //     {
        //     //         KeyModifiers |= ConsoleModifiers.Control;
        //     //         break;
        //     //     }
        //     // }
        // }
    }    

    public ConsoleKey KeyType { get; }
    
    public ConsoleModifiers KeyModifiers { get; } = ConsoleModifiers.None;
}