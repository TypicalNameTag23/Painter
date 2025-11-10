using Taui.Composeables;
using Taui.Drawing;
using Taui.Exceptions;
using Taui.Utilities;

namespace Taui;

/// <summary>
/// A Taui Application.
/// </summary>
public class Application //todo: make singleton or implement TopLevel like Avalonia?
{
    private int _targetFps;
    private int _timeBetweenFrames;
    private Canvas _canvas;

    /// <summary>
    /// The target FPS for this <see cref="Application"/>.
    /// </summary>
    public int TargetFps
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
    public Window Window { get; set; }
    
    /// <summary>
    /// This <see cref="Application"/>'s current <see cref="ApplicationState"/>.
    /// </summary>
    public ApplicationState State { get; private set; } = ApplicationState.Inactive;

    /// <summary>
    /// Get this <see cref="Application"/> window's width
    /// </summary>
    public int Width => Console.BufferWidth;
    
    /// <summary>
    /// Get this <see cref="Application"/> window's height
    /// </summary>
    public int Height => Console.BufferHeight;

    public event ConsoleCancelEventHandler? OnQuitKeyPress;
    
    public event ConsoleKeyEventHandler? OnKeyPress;
    
    public static event FrameRenderEventHandler? BeforeFrameRender;
    
    public static event FrameRenderEventHandler? AfterFrameRender;

    /// <summary>
    /// Starts this <see cref="Application"/> if it hasn't already been started.
    /// </summary>
    /// <exception cref="TauiException">Thrown if this <see cref="Application"/> is already started or if the <see cref="Window"/> hasn't been set.</exception>
    public void Start()
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
    private void Display()
    {
        BeforeFrameRender?.Invoke();
        if (_canvas.Width != Console.BufferWidth || _canvas.Height != Console.BufferHeight)
        {
            _canvas = new Canvas(Width, Height);
            Console.Clear();
        } 
        //todo: dont redraw the entire app every frame when the elements dont change?
        //todo: dont print anything if the pixel is nothing, unless the previous frame had something at that pixel?
        
        Window.ComposeOnto(_canvas);
        Console.SetCursorPosition(0, 0);
        for (var y = 0; y < _canvas.Height; y++)
        {
            for (var x = 0; x < _canvas.Width; x++)
            {
                ConsoleUtils.Write(_canvas.View(x, y));
            }

            if (y != _canvas.Height - 1)
            {
                Console.WriteLine();
            }
        }

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
