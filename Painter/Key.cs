namespace Painter;

public record Key
{
    public Key(ConsoleKey key, char character, ConsoleModifiers modifier = ConsoleModifiers.None)
    {
        KeyType = key;
        Character = character;
        KeyModifiers = modifier;
    }    

    public ConsoleKey KeyType { get; }
    
    public ConsoleModifiers KeyModifiers { get; } = ConsoleModifiers.None;
    
    public char Character { get; }
}