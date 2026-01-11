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

    public Application()
    {
        _terminal = Terminal.GetPlatformTerminal();
        _mainWindow = new Window
        {
            Width = _terminal.Width,
            Height = _terminal.Height
        };
        
        _canvas = new TerminalCanvas(_terminal, Width, Height);
        _terminal.TerminalSizeChangedEvent += (_, args) =>
        {
            Console.Write(AnsiTerminalCodes.ClearScreen);
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
        set => _mainWindow.Content = value;
    }
    
    public void AddGlobalKeyBind(Key key, Action run)
    {
        _mainWindow.AddKeyBind(key, run);
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
        
        Console.Write(
            AnsiTerminalCodes.SaveScreen + 
            AnsiTerminalCodes.SaveCursorPositon + 
            AnsiTerminalCodes.ClearScreen +
            AnsiTerminalCodes.HideCursor +
            AnsiTerminalCodes.ResetCursorPosition + 
            AnsiTerminalCodes.ResetStyles);
        
        Console.TreatControlCAsInput = true;
        ApplicationState = ApplicationState.Running;
        while (ApplicationState == ApplicationState.Running)
        {
            _terminal.PollSizeChange();
            if (_mainWindow.NeedsRedraw)
            {
                _mainWindow.Draw(_canvas);
                _canvas.Display();
                _mainWindow.NeedsRedraw = false;
            }
            
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
    }
}