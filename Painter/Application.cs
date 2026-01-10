using Painter.Drawing;
using Painter.Elements;
using Painter.Platform;

namespace Painter;

public class Application
{
    private readonly ApplicationContext _context = new();
    private readonly MainWindow _mainWindow;
    private readonly Canvas _canvas;

    public Application()
    {
        _canvas = new Canvas(_context.Terminal, _context.Width, _context.Height);
        _mainWindow = new MainWindow(_context);
    }

    public Element? Drawable
    {
        get => _mainWindow.Drawable;
        set => _mainWindow.Drawable = value;
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
        if (_context.ApplicationState != ApplicationState.Inactive)
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
        // new Thread(_ =>
        // {
        //     while (_context.ApplicationState == ApplicationState.Running)
        //     {
        //         
        //     }
        // }).Start();
        
        _context.ApplicationState = ApplicationState.Running;
        while (_context.ApplicationState == ApplicationState.Running)
        {
            CheckPrintableAreaChanged();
            if (_context.NeedsPartialRedraw || _context.NeedsFullRedraw)
            {
                Display();
            }
            
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                _mainWindow.ExecuteWindowKeyBinds(new Key(key.Key, key.Modifiers));
            }
        }
    }

    public void Kill()
    {
        _context.ApplicationState = ApplicationState.Killed;
    }

    private void Display()
    {
        _mainWindow.Draw(_canvas, _context.NeedsFullRedraw);
        _canvas.Display();
        _context.NeedsPartialRedraw = false;
        _context.NeedsFullRedraw = false;
    }

    private void CheckPrintableAreaChanged()
    {
        var width = _context.Width;
        var height = _context.Height;
        if (_mainWindow.Width == width && _mainWindow.Height == height)
        {
            return;
        }
        
        Console.Write(AnsiTerminalCodes.ClearScreen);
        _mainWindow.Width = _context.Width;
        _mainWindow.Height = _context.Height;
        _canvas.Resize(_context.Width, _context.Height);
        _context.NeedsFullRedraw = true;
    }
}