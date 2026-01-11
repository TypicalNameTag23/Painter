using System.Runtime.InteropServices;
using Painter.Drawables.DrawableImpl;

namespace Painter.Drawables;

public abstract class TopLevel : ContentControl
{
    private readonly Dictionary<Key, List<Action>> _windowKeyBinds = [];

    public void AddWindowKeyBind(Key key, Action action)
    {
        if (!_windowKeyBinds.TryGetValue(key, out var actions))
        {
            actions = [];
            _windowKeyBinds.Add(key, actions);
        }
        
        actions.Add(action);
    }
    
    public void RemoveWindowKeyBinds(Key key)
    {
        _windowKeyBinds.Remove(key);
    }
    
    public void ExecuteWindowKeyBinds(Key key)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            key = new Key(key.KeyType, key.KeyModifiers ^ ConsoleModifiers.Shift);
        }
        
        if (!_windowKeyBinds.TryGetValue(key, out var actions))
        {
            return;
        }
        
        foreach (var action in actions)
        {
            action();
        }
    }
    
    public IReadOnlyList<Key> GetWindowBoundKeys()
    {
        return _windowKeyBinds.Keys.ToList();
    }
}