using Taui.Composeables;
using Taui.Drawing;
using Taui.Exceptions;
using Taui.Utilities;

namespace Taui;

/// <summary>
/// A Painter Application.
/// </summary>
public static class Application //todo: make singleton or implement TopLevel like Avalonia?
{
    private static int _targetFps;
    private static int _timeBetweenFrames;
    private static Canvas _canvas;
    private static Canvas _priorCanvas;

    /// <summary>
    /// The target FPS for this <see cref="Application"/>.
    /// </summary>
    public static int TargetFps
    {
        get => _targetFps;
        set
        {
            _targetFps = value;
            if (value > -1)
            {
                _timeBetweenFrames = 1000 / value;
            }
            else
            {
                _timeBetweenFrames = 0;
            }
        }
    }
    
    /// <summary>
    /// The <see cref="Window"/> for this <see cref="Application"/> to display.
    /// </summary>
    public static Window Window { get; set; }
    
    /// <summary>
    /// This <see cref="Application"/>'s current <see cref="ApplicationState"/>.
    /// </summary>
    public static ApplicationState State { get; private set; } = ApplicationState.Inactive;

    /// <summary>
    /// Get this <see cref="Application"/> window's width
    /// </summary>
    public static int Width => Console.BufferWidth;
    
    /// <summary>
    /// Get this <see cref="Application"/> window's height
    /// </summary>
    public static int Height => Console.BufferHeight;

    public static event ConsoleCancelEventHandler? OnQuitKeyPress;
    
    public static event ConsoleKeyEventHandler? OnKeyPress;
    
    public static event FrameRenderEventHandler? BeforeFrameRender;
    
    public static event FrameRenderEventHandler? AfterFrameRender;

    /// <summary>
    /// Starts this <see cref="Application"/> if it hasn't already been started.
    /// </summary>
    /// <exception cref="TauiException">Thrown if this <see cref="Application"/> is already started or if the <see cref="Window"/> hasn't been set.</exception>
    public static void Start()
    {
        if (State != ApplicationState.Inactive)
        {
            throw new TauiException("Can't start application when it is already active.");
        }

        if (Window is null)
        {
            throw new TauiException("Window must be set before application is started.");
        }
        
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
        _priorCanvas = new Canvas(Width, Height);
        State = ApplicationState.Running;
        while (State == ApplicationState.Running)
        {
            if (Console.KeyAvailable)
            {
                OnKeyPress?.Invoke(Console.ReadKey());
            }
            
            Display();
            Thread.Sleep(_timeBetweenFrames);
        }
    }

    /// <summary>
    /// Displays a frame of this <see cref="Application"/> to the console.
    /// </summary>
    private static void Display()
    {
        BeforeFrameRender?.Invoke();
        if (_canvas.Width != Console.BufferWidth || _canvas.Height != Console.BufferHeight)
        {
            _canvas.Resize(Width, Height);
            _priorCanvas.Resize(Width, Height);
            _priorCanvas.Clear();
            Console.Clear();
        } 
        //todo: dont redraw the entire app every frame when the elements dont change?
        //todo: dont print anything if the pixel is nothing, unless the previous frame had something at that pixel?
        
        Window.DrawOnto(_canvas);
        Console.SetCursorPosition(0, 0);
        _canvas.RunOnPixels((point, pixel) =>
        {
            if (_priorCanvas.View(point) != pixel)
            {
                ConsoleUtils.WriteTo(point, pixel);
            }
            
            _priorCanvas.Erase(point);
        });
        
        _priorCanvas.RunOnPixels((point, _) => ConsoleUtils.WriteTo(point, null));
        (_priorCanvas, _canvas) = (_canvas, _priorCanvas);
        _canvas.Clear();
        AfterFrameRender?.Invoke();
    }
}

/// <summary>
/// The <see cref="Application"/>'s state.
/// </summary>
public enum ApplicationState
{
    Inactive,
    Running,
    Killed
}
