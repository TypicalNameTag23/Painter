namespace Painter.Interactions;

public class InteractionManager
{
    private List<Interactable> _interactables = [];
    private int _focused;

    public Interactable Focused
    {
        get => _interactables[_focused];
    }

    public void CycleFocus()
    {
        _focused += 1;
        
        if (_focused == _interactables.Count)
        {
            _focused = 0;
        }
    }
}