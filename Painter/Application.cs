using Painter.Components;
using Painter.Drawing;
using Painter.Events;
using Painter.Exceptions;

namespace Painter;

public static class Application 
{
    private static int _targetFramerate = -1;
    private static int _timeBetweenFrames;
    private static Canvas _canvas;
    
    public static int TargetFramerate
    {
        get => _targetFramerate;
        set
        {
            _targetFramerate = value;
            if (value > 0)
            {
                _timeBetweenFrames = 1000 / value;
            }
            else
            {
                _timeBetweenFrames = 0;
            }
        }
    }
    
    public static Window Window { get; set; }
    
    public static ApplicationState State { get; private set; } = ApplicationState.Inactive;
    
    public static int Width => Console.BufferWidth;
    
    public static int Height => Console.BufferHeight;
    
    public static event ConsoleCancelEventHandler? OnQuitKeyPress;
    
    public static event ConsoleKeyEventHandler? OnKeyPress;
    
    public static event FrameRenderEventHandler? BeforeFrameRender;
    
    public static event FrameRenderEventHandler? AfterFrameRender;
    
    public static void Start()
    {
        // var s = "asd" + AnsiTerminalCodes.SetCursorPosition(0, 0) + "1111";
        // Console.Write(s);
        //
        // return;
        ValidateCanStart();
        Console.Clear();
        Console.CursorVisible = false;
        Console.CancelKeyPress += (sender, args) =>
        {
            OnQuitKeyPress?.Invoke(sender, args);
            if (!args.Cancel)
            {
                State = ApplicationState.Killed;
                Console.Clear();
            }
        };
        
        _canvas = new Canvas(Width, Height);
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
            Display();
            Thread.Sleep(_timeBetweenFrames); // is there a better way of doing this?
        }
    }

    private static void ValidateCanStart()
    {
        if (State != ApplicationState.Inactive)
        {
            throw new PainterException("Can't start application when it is already active.");
        }

        if (Window is null)
        {
            throw new PainterException("Window must be set before application is started.");
        }
    }

    private static void Display()
    {
        BeforeFrameRender?.Invoke();
        CheckPrintableAreaChanged();
        Window.DrawOnto(_canvas);
        _canvas.Display();
        AfterFrameRender?.Invoke();
    }

    private static void CheckPrintableAreaChanged()
    {
        if (_canvas.Width == Width && _canvas.Height == Height)
        {
            return;
        }

        _canvas.Resize(Width, Height);
    }
    
}