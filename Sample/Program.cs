using Painter;
using Painter.Components;

namespace Sample;

static class Program
{
    static void Main()
    {
        Application.TargetFramerate = -1;
        Application.OnQuitKeyPress += (_, args) =>
        {
            args.Cancel = false;
        };
        
        var mainWindow = new Window
        {
            Drawable = new FpsTacker()
            {
                Blinking = false
            }
        };

        Application.OnKeyPress += consoleKeyInfo =>
        {
            if (consoleKeyInfo.Key == ConsoleKey.Spacebar)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Pressed Space!");
            }
        };
        
        Application.Window = mainWindow;
        Application.Start();
    }
}