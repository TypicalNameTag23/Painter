using Painter.Drawing;
using Painter.Elements;
using Painter.Events;

namespace Painter;

public class Application
{
    private MainWindow _mainWindow;
    private int _timeBetweenFrames;
    private Canvas _canvas;
    private bool _needsRedraw = true;
    private bool _needsFullRedraw = true;

    public Application()
    {
        _canvas = new Canvas(Width, Height);
        _mainWindow = new MainWindow
        {
            Application = this,
            Width = Width,
            Height = Height
        };
    }
    
    public int TargetFramerate
    {
        get;
        set
        {
            field = value;
            if (value > 0)
            {
                TargetFramerate = 1000 / value;
            }
            else
            {
                _timeBetweenFrames = 0;
            }
        }
    }

    public Element? Drawable
    {
        get => _mainWindow.Drawable;
        set => _mainWindow.Drawable = value;
    }
    
    public ApplicationState State { get; private set; } = ApplicationState.Inactive;
    
    public int Width => Console.BufferWidth;
    
    public int Height => Console.BufferHeight;
    
    // handle better
    public static event ConsoleCancelEventHandler? OnQuitKeyPress;
    
    public static event ConsoleKeyEventHandler? OnKeyPress;
    
    public static event FrameRenderEventHandler? BeforeFrameRender;
    
    public static event FrameRenderEventHandler? AfterFrameRender;
    
    public void Start()
    {
        Console.Write(
            AnsiTerminalCodes.SaveScreen + 
            AnsiTerminalCodes.SaveCursorPositon + 
            AnsiTerminalCodes.ClearDisplay +
            AnsiTerminalCodes.HideCursor +
            AnsiTerminalCodes.SetCursorPosition(0, 0) + 
            AnsiTerminalCodes.Reset);
        
        Console.CancelKeyPress += (sender, args) =>
        {
            OnQuitKeyPress?.Invoke(sender, args);
            if (args.Cancel)
            {
                return;
            }
            
            State = ApplicationState.Killed;
            Console.Write(
                AnsiTerminalCodes.Reset + 
                AnsiTerminalCodes.ClearDisplay + 
                AnsiTerminalCodes.ResetForegroundColor +
                AnsiTerminalCodes.ResetBackgroundColor +
                AnsiTerminalCodes.ShowCursor + 
                AnsiTerminalCodes.RestoreCursorPositon + 
                AnsiTerminalCodes.RestoreScreen);
        };
        
        State = ApplicationState.Running;
        new Thread(_ =>
        {
            while (State == ApplicationState.Running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    OnKeyPress?.Invoke(key);
                }
            }
        }).Start();
        
        while (State == ApplicationState.Running)
        {
            BeforeFrameRender?.Invoke();
            CheckPrintableAreaChanged();
            if (_needsRedraw || _needsFullRedraw)
            {
                Display();
            }
            AfterFrameRender?.Invoke();
            
            // Thread.Sleep(_timeBetweenFrames); // is there a better way of doing this?
        }
    }

    public void RequestRedraw(Element element)
    {
        _needsRedraw = true;
    }

    private void Display()
    {
        _mainWindow.Draw(_canvas, _needsFullRedraw);
        _canvas.Display();
        _needsRedraw = false;
        _needsFullRedraw = false;
    }

    private void CheckPrintableAreaChanged()
    {
        if (_mainWindow.Width == Width && _mainWindow.Height == Height)
        {
            return;
        }

        _mainWindow.Width = Width;
        _mainWindow.Height = Height;
        _canvas.Resize(Width, Height);
        _needsFullRedraw = true;
        Console.Write(AnsiTerminalCodes.ClearDisplay); 
    }
    
}

public enum ApplicationState
{
    Inactive,
    Running,
    Killed
}