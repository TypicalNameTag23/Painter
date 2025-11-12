namespace Painter.Interactions;

public abstract class Interactable
{
    private bool _focused;

    internal bool Focused
    {
        get => _focused;
        set => _focused = value;
    }

    public abstract void Interact(ConsoleKeyInfo info);
}