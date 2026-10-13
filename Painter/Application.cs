using Painter.Drawables;
using Painter.Drawing.Canvas;
using Painter.Events;
using Painter.Platform;

namespace Painter;

public class Application
{
    private readonly Terminal _terminal;
    private readonly Window _mainWindow;
    private readonly TerminalCanvas _canvas;
    
    public static event BeforeApplicationDrawFrameEvent? BeforeApplicationDrawFrameEvent;
    public static event AfterApplicationDrawFrameEvent? AfterApplicationDrawFrameEvent;

    public Application(Terminal? terminal = null)
    {
        _terminal = terminal ?? Terminal.GetPlatformTerminal();
        _mainWindow = new Window(this)
        {
            Width = _terminal.Width,
            Height = _terminal.Height
        };

        _canvas = new TerminalCanvas(_terminal, Width, Height);
        _terminal.TerminalSizeChangedEvent += (_, args) =>
        {
            _terminal.Write(AnsiTerminalCodes.ClearScreen);
            _mainWindow.Width = args.Width;
            _mainWindow.Height = args.Height;
            _mainWindow.NeedsRedraw = true;
        };
    }

    public int Width => _mainWindow.Width;
    public int Height => _mainWindow.Height;

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

    public Drawable? Content
    {
        get => _mainWindow.Content;
        set
        {
            _mainWindow.Content = value;
        }
    }
    
    public object? DataContext
    {
        get => _mainWindow.DataContext;
        set => _mainWindow.DataContext = value;
    }
    
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
        
        _terminal.Write(
            // AnsiTerminalCodes.FuckAssLinuxThingStolenFromTerminalDotGui +
            AnsiTerminalCodes.DisableLineWrapping +
            AnsiTerminalCodes.SaveScreen + 
            AnsiTerminalCodes.SaveCursorPositon + 
            AnsiTerminalCodes.ClearScreen +
            AnsiTerminalCodes.HideCursor +
            // AnsiTerminalCodes.CSI_EnableAnyEventMouse +
            // AnsiTerminalCodes.CSI_EnableSgrExtModeMouse +
            // AnsiTerminalCodes.CSI_EnableUrxvtExtModeMouse +
            AnsiTerminalCodes.ResetCursorPosition + 
            AnsiTerminalCodes.ResetStyles);
        
        //Console.TreatControlCAsInput = true;
        ApplicationState = ApplicationState.Running;
        // _keyProcessingTimer = new Timer(_ =>
        // {
        //     
        // }, null, 5, 5);
        
        while (ApplicationState == ApplicationState.Running)
        {
            var key = _terminal.ReadKey();
            while (key != null)
            {
                _mainWindow.Interact(new KeyPressedEventArgs(key.Key, key.Character));
                key = _terminal.ReadKey();
            }
            
            _terminal.PollSizeChange();
            if (_mainWindow.NeedsRedraw)
            {
                BeforeApplicationDrawFrameEvent?.Invoke();
                _mainWindow.Draw(_canvas);
                _mainWindow.NeedsRedraw = false;
                
                _canvas.Display();
                AfterApplicationDrawFrameEvent?.Invoke();
            }
        }
    }

    public void Kill()
    {
        ApplicationState = ApplicationState.Killed;
        _terminal.Write(AnsiTerminalCodes.EnableLineWrapping);
    }
}