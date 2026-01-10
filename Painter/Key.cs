namespace Painter;

public class Key
{
    public Key(ConsoleKey key, params ConsoleModifiers[] modifiers)
    {
        KeyType = key;
        foreach (var modifier in modifiers)
        {
            KeyModifiers |= modifier;
        }
    }

    public ConsoleKey KeyType { get; }
    
    public ConsoleModifiers KeyModifiers { get; } = ConsoleModifiers.None;

    public override bool Equals(object? other)
    {
        if (other is null)
        {
            return false;
        }
        
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other switch
        {
            Key key => key.KeyType == KeyType && key.KeyModifiers == KeyModifiers,
            ConsoleKeyInfo keyInfo => keyInfo.Key == KeyType && keyInfo.Modifiers == KeyModifiers,
            _ => false
        };
    }

    protected bool Equals(Key other)
    {
        return KeyType == other.KeyType && KeyModifiers == other.KeyModifiers;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int) KeyType, (int) KeyModifiers);
    }
}