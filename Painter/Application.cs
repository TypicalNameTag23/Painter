using Painter.Components;
using Painter.Drawing;
using Painter.Events;
using Painter.Exceptions;
using Painter.Utilities;

namespace Painter;

// todo: Should this be static, is this a bad design pattern?
/// <summary>
/// A Painter Application.
/// </summary>
public static class Application 
{
    private static int _targetFps = -1;
    private static int _timeBetweenFrames;
    private static Canvas _canvas;
    private static Canvas _priorCanvas;

    /// <summary>
    /// The target FPS for this <see cref="Application"/>, -1 for no cap.
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

    /// <summary>
    /// Event fired when the quit key (like Ctrl+C) is pressed, can be canceled.
    /// </summary>
    public static event ConsoleCancelEventHandler? OnQuitKeyPress;
    
    /// <summary>
    /// Event fired when a key is pressed.
    /// </summary>
    public static event ConsoleKeyEventHandler? OnKeyPress;
    
    /// <summary>
    /// Event fired before a frame is rendered.
    /// </summary>
    public static event FrameRenderEventHandler? BeforeFrameRender;
    
    /// <summary>
    /// Event fired after a frame is rendered.
    /// </summary>
    public static event FrameRenderEventHandler? AfterFrameRender;

    /// <summary>
    /// Starts this <see cref="Application"/> if it hasn't already been started.
    /// </summary>
    /// <exception cref="PainterException">Thrown if this <see cref="Application"/> is already started or if the <see cref="Window"/> hasn't been set.</exception>
    public static void Start()
    {
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
        _priorCanvas = new Canvas(Width, Height);
        State = ApplicationState.Running;
        while (State == ApplicationState.Running)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                OnKeyPress?.Invoke(key);
            }
            
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
        Console.SetCursorPosition(0, 0);
        _canvas.RunOnDrawnPixels((point, pixel) =>
        {
            if (_priorCanvas.View(point) != pixel) // Don't need to print again if the pixel hasn't changed
            {
                try
                {
                    ConsoleUtils.WriteTo(point, pixel);
                }
                catch (PainterException)
                {
                    return; // skip frame if user is in the middle of resizing terminal
                }
            }
            
            _priorCanvas.Erase(point); // Remove the pixel from the previous canvas if there was one, since it has already been overriden
        });
        
        _priorCanvas.RunOnDrawnPixels((point, _) =>
        {
            try
            {
                ConsoleUtils.WriteTo(point, null);
            }
            catch (PainterException)
            {
                return; // skip frame if user is in the middle of resizing terminal
            }
        }); // Override all remaining pixels from the previous canvas
        (_priorCanvas, _canvas) = (_canvas, _priorCanvas); // Swap canvases
        _canvas.Clear();
        AfterFrameRender?.Invoke();
    }

    private static void CheckPrintableAreaChanged()
    {
        if (_canvas.Width == Width && _canvas.Height == Height)
        {
            return;
        }
        
        Console.Clear();
        _canvas.Resize(Width, Height);
        _priorCanvas.Resize(Width, Height);
        _priorCanvas.Clear();
    }
    
}