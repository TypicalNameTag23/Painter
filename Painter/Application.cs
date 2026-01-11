using Painter.Drawables;
using Painter.Drawing;
using Painter.Events;
using Painter.Platform;

namespace Painter;

public class Application
{
    private readonly Terminal _terminal = Terminal.GetPlatformTerminal();
    private readonly Window _mainWindow;
    private readonly TerminalCanvas _canvas;

    public Application()
    {
        _canvas = new TerminalCanvas(_terminal, Width, Height);
        _mainWindow = new Window();
    }

    public static int Width => Terminal.Width;

    public static int Height => Terminal.Height;

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
        _mainWindow.AddWindowKeyBind(key, run);
    }
    
    public void RemoveGlobalKeyBind(Key key)
    {
        _mainWindow.RemoveWindowKeyBinds(key);
    }
    
    public IReadOnlyList<Key> GetGlobalBoundKeys()
    {
        return _mainWindow.GetWindowBoundKeys();
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
            CheckPrintableAreaChanged();
            if (_mainWindow.NeedsRedraw)
            {
                DrawAndDisplay();
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

    private void DrawAndDisplay()
    {
        _mainWindow.DrawImpl(_canvas);
        _canvas.Display();
        _mainWindow.NeedsRedraw = false;
    }

    private void CheckPrintableAreaChanged()
    {
        var width = Width;
        var height = Height;
        if (_mainWindow.Width == width && _mainWindow.Height == height)
        {
            return;
        }
        
        Console.Write(AnsiTerminalCodes.ClearScreen);
        _mainWindow.Width = width;
        _mainWindow.Height = height;
        _canvas.Resize(width, height);
        _mainWindow.NeedsRedraw = true;
    }
}

public enum ApplicationState
{
    Inactive,
    Running,
    Killed
}