using Painter.Platform;

namespace Painter.Events;

public class KeyPressedEventArgs(Key key, char character) : EventArgs
{
    public Key Key { get; } = key;
    public char Character { get; } = character;
}