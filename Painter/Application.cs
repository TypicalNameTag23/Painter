using Painter.Drawables;
using Painter.Events;
using Painter.Platform;
using Painter.Rendering;

namespace Painter;

public class Application
{
    private Timer _keyProcessingTimer;
    private Renderer _renderer;
    private Window _mainWindow;
    private Terminal _terminal;

    public static event BeforeApplicationDrawFrameEvent? BeforeApplicationDrawFrameEvent;
    public static event AfterApplicationDrawFrameEvent? AfterApplicationDrawFrameEvent;

    public Window MainWindow { get; set; }

    public ApplicationState ApplicationState
    {
        get;
        private set
        {
            if (field != ApplicationState.Inactive && value == ApplicationState.Inactive)
            {
                return;
            }

            field = value;
        }
    } = ApplicationState.Inactive;
    
    public void AddGlobalKeyBind(Key key, EventHandler<KeyPressedEventArgs> eventHandler)
    {
        _mainWindow.AddKeyBind(key, eventHandler);
    }
    
    public void RemoveGlobalBoundKey(Key key)
    {
        _mainWindow.RemoveBoundKey(key);
    }
    
    public IReadOnlyList<Key> GetGlobalBoundKeys()
    {
        return _mainWindow.GetBoundKeys();
    }
    
    public void Start()
    {
        if (ApplicationState != ApplicationState.Inactive)
        {
            return;
        }

        _terminal = Terminal.GetPlatformTerminal();
        _renderer = new Renderer(_terminal, _mainWindow);
        
        _terminal.Initialize();
        ApplicationState = ApplicationState.Running;
        while (ApplicationState == ApplicationState.Running)
        {
            _terminal.PollSizeChange();   
            _renderer.ExecuteRenderPass();
            
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                _mainWindow.Interact(new KeyPressedEventArgs(new Key(key.Key, key.Modifiers), key.KeyChar));
            }
        }
    }

    public void Kill()
    {
        ApplicationState = ApplicationState.Killed;
        _keyProcessingTimer.Dispose();
        Console.WriteLine(AnsiTerminalCodes.EnableLineWrapping);
    }
}