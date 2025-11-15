using Painter.Drawing;
using Painter.Elements;
using Painter.Events;
using Painter.Platform;

namespace Painter;

public class Application
{
    private MainWindow _mainWindow;
    private Terminal _terminal = Terminal.GetPlatformTerminal();
    private int _timeBetweenFrames;
    private Canvas _canvas;
    private bool _needsRedraw = true;
    private bool _needsFullRedraw = true;

    public Application()
    {
        _canvas = new Canvas(_terminal, Width, Height);
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

#if DEBUG
    public static event FrameRenderEventHandler? BeforeDraw;
    
    public static event FrameRenderEventHandler? AfterDraw;
#endif
    
    
    
    public void Start()
    {
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
            
            State = ApplicationState.Killed;
            Console.Write(
                AnsiTerminalCodes.ResetStyles + 
                AnsiTerminalCodes.ClearScreen + 
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
                // BeforeFrameRender?.Invoke();
                Display();
                // AfterFrameRender?.Invoke();
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
#if DEBUG
        BeforeDraw?.Invoke();
#endif
        _mainWindow.Draw(_canvas, _needsFullRedraw);
#if DEBUG
        AfterDraw?.Invoke();
#endif
        _canvas.Display();
        _needsRedraw = false;
        _needsFullRedraw = false;
    }

    private void CheckPrintableAreaChanged()
    {
        var width = Width;
        var height = Height;
        if (_mainWindow.Width == width && _mainWindow.Height == height)
        {
            return;
        }

        if (width < _mainWindow.Width || height < _mainWindow.Height)
        {
            Console.Write(AnsiTerminalCodes.ClearScreen);
        }
        
        _mainWindow.Width = Width;
        _mainWindow.Height = Height;
        _canvas.Resize(Width, Height);
        _needsFullRedraw = true;
    }
    
}

public enum ApplicationState
{
    Inactive,
    Running,
    Killed
}