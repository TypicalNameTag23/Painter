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
        _mainWindow.RemoveWindowKeyBind(key);
    }
    
    public IReadOnlyList<KeyBind> GetGlobalKeyBinds(Key key)
    {
        return _mainWindow.GetWindowKeyBinds(key);
    }
    
    public IReadOnlyList<KeyBind> GetGlobalKeyBinds()
    {
        return _mainWindow.GetWindowKeyBinds();
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
        
        Console.CancelKeyPress += (_, args) =>
        {
            args.Cancel = true;
            // _context.ApplicationState = ApplicationState.Killed;
            // Console.Write(
            //     AnsiTerminalCodes.ResetStyles + 
            //     AnsiTerminalCodes.ClearScreen + 
            //     AnsiTerminalCodes.ResetForegroundColor +
            //     AnsiTerminalCodes.ResetBackgroundColor +
            //     AnsiTerminalCodes.ShowCursor + 
            //     AnsiTerminalCodes.RestoreCursorPositon + 
            //     AnsiTerminalCodes.RestoreScreen);
        };
        
        new Thread(_ =>
        {
            while (_context.ApplicationState == ApplicationState.Running)
            {
                if (!Console.KeyAvailable)
                {
                    continue;
                }
                
                foreach (var keyBind in _mainWindow.GetWindowKeyBinds(Console.ReadKey(true)))
                {
                    keyBind.Run();
                }
            }
        }).Start();
        
        _context.ApplicationState = ApplicationState.Running;
        while (_context.ApplicationState == ApplicationState.Running)
        {
            CheckPrintableAreaChanged();
            if (_context.NeedsPartialRedraw || _context.NeedsFullRedraw)
            {
                Display();
            }
        }
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