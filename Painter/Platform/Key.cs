namespace Painter;

public record Key
{
    public Key(ConsoleKey key, ConsoleModifiers modifier = ConsoleModifiers.None)
    {
        KeyType = key;
        KeyModifiers = modifier;
    }    

    public ConsoleKey KeyType { get; }
    
    public ConsoleModifiers KeyModifiers { get; } = ConsoleModifiers.None;
}