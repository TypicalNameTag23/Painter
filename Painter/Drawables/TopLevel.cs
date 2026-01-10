namespace Painter.Drawables;

public abstract class TopLevel
{
    private readonly List<KeyBind> _windowKeyBinds = [];

    public void AddWindowKeyBind(Key key, Action action)
    {
        _windowKeyBinds.Add(new KeyBind(key, action));
    }
    
    public void RemoveWindowKeyBind(KeyBind keyBind)
    {
        _windowKeyBinds.Remove(keyBind);
    }
    
    public void RemoveWindowKeyBind(Key key)
    {
        for (var i = 0; i < _windowKeyBinds.Count; i++)
        {
            if (_windowKeyBinds[i].Key.Equals(key))
            {
                continue;
            }

            _windowKeyBinds.RemoveAt(i);
        }
    }
    
    public IReadOnlyList<KeyBind> GetWindowKeyBinds(Key key)
    {
        var keyBinds = new List<KeyBind>();
        foreach (var keyBind in _windowKeyBinds)
        {
            if (keyBind.Key.Equals(key))
            {
                continue;
            }

            keyBinds.Add(keyBind);
        }

        return keyBinds.AsReadOnly();
    }
    
    public List<KeyBind> GetWindowKeyBinds(ConsoleKeyInfo key)
    {
        var keyBinds = new List<KeyBind>();
        foreach (var keyBind in _windowKeyBinds)
        {
            if (keyBind.Key.Equals(key))
            {
                continue;
            }

            keyBinds.Add(keyBind);
        }

        return keyBinds;
    }
    
    public IReadOnlyList<KeyBind> GetWindowKeyBinds()
    {
        return _windowKeyBinds.AsReadOnly();
    }
}