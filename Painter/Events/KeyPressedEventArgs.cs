namespace Painter.Events;

public class KeyPressedEventArgs(Key key) : EventArgs
{
    public Key Key { get; } = key;
}