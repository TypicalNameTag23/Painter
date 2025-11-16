using Painter.Drawing;
using Painter.Elements;
using Painter.Events;
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
    
    // handle better
    public static event ConsoleCancelEventHandler? OnQuitKeyPress;
    
    public static event ConsoleKeyEventHandler? OnKeyPress;
    
    public static event FrameRenderEventHandler? BeforeFrameRender;

    public static event FrameRenderEventHandler? AfterFrameRender;
    
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
        
        Console.CancelKeyPress += (sender, args) =>
        {
            OnQuitKeyPress?.Invoke(sender, args);
            if (args.Cancel)
            {
                return;
            }
            
            _context.ApplicationState = ApplicationState.Killed;
            Console.Write(
                AnsiTerminalCodes.ResetStyles + 
                AnsiTerminalCodes.ClearScreen + 
                AnsiTerminalCodes.ResetForegroundColor +
                AnsiTerminalCodes.ResetBackgroundColor +
                AnsiTerminalCodes.ShowCursor + 
                AnsiTerminalCodes.RestoreCursorPositon + 
                AnsiTerminalCodes.RestoreScreen);
        };
        
        _context.ApplicationState = ApplicationState.Running;
        new Thread(_ =>
        {
            while (_context.ApplicationState == ApplicationState.Running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    OnKeyPress?.Invoke(key);
                }
            }
        }).Start();
        
        while (_context.ApplicationState == ApplicationState.Running)
        {
            BeforeFrameRender?.Invoke();
            CheckPrintableAreaChanged();
            if (_context.NeedsPartialRedraw || _context.NeedsFullRedraw)
            {
                Display();
            }
            AfterFrameRender?.Invoke();
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

public enum ApplicationState
{
    Inactive,
    Running,
    Killed
}