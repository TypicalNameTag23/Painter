using System.Runtime.InteropServices;
using Painter.Events;
using Painter.Rendering;

namespace Painter.Drawables;

public abstract class TopLevel(Application applicationContext) : ContentControl
{
    internal ILayoutManager LayoutManager { get; }
    internal IRenderer Renderer => (Renderer) LayoutManager;
    
    private readonly Dictionary<Key, List<EventHandler<KeyPressedEventArgs>>> _globalKeyBinds = [];

    public void AddKeyBind(Key key, EventHandler<KeyPressedEventArgs> eventHandler)
    {
        if (!_globalKeyBinds.TryGetValue(key, out var eventHandlers))
        {
            eventHandlers = [];
            _globalKeyBinds.Add(key, eventHandlers);
        }
        
        eventHandlers.Add(eventHandler);
    }
    
    public void RemoveBoundKey(Key key)
    {
        _globalKeyBinds.Remove(key);
    }
    
    public void ExecuteKeyBinds(KeyPressedEventArgs args)
    {
        var key = args.Key;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Console.CapsLock)
        {
            key = new Key(key.KeyType, key.KeyModifiers ^ ConsoleModifiers.Shift);
        }
        
        if (!_globalKeyBinds.TryGetValue(key, out var eventHandlers))
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
        return _globalKeyBinds.Keys.ToList();
    }
}