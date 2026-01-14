using System.Runtime.InteropServices;
using Painter.Drawables.DrawableImpl;
using Painter.Events;

namespace Painter.Drawables;

public abstract class TopLevel(Application applicationContext) : ContentControl
{
    public Application ApplicationContext { get; } = applicationContext;
    
    private readonly Dictionary<Key, List<EventHandler<KeyPressedEventArgs>>> _windowKeyBinds = [];

    public void AddKeyBind(Key key, EventHandler<KeyPressedEventArgs> eventHandler)
    {
        if (!_windowKeyBinds.TryGetValue(key, out var eventHandlers))
        {
            eventHandlers = [];
            _windowKeyBinds.Add(key, eventHandlers);
        }
        
        eventHandlers.Add(eventHandler);
    }
    
    public void RemoveBoundKey(Key key)
    {
        _windowKeyBinds.Remove(key);
    }
    
    public void ExecuteKeyBinds(KeyPressedEventArgs args)
    {
        var key = args.Key;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Console.CapsLock)
        {
            key = new Key(key.KeyType, key.KeyModifiers ^ ConsoleModifiers.Shift);
        }
        
        if (!_windowKeyBinds.TryGetValue(key, out var eventHandlers))
        {
            return;
        }
        
        foreach (var eventHandler in eventHandlers)
        {
            eventHandler.Invoke(this, args);
        }
    }
    
    public IReadOnlyList<Key> GetBoundKeys()
    {
        return _windowKeyBinds.Keys.ToList();
    }
}