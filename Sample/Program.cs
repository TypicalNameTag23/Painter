using System.Runtime.InteropServices.ComTypes;
using Taui;
using Taui.Composeables;

namespace Sample;

class Program
{
    static void Main()
    {
        Application.TargetFps = -1;
        Application.OnQuitKeyPress += (_, args) =>
        {
            args.Cancel = false;
        };
        
        var mainWindow = new Window
        {
            Drawable = new FpsTacker
            {
                Blinking = true
            }
        };

        Application.OnKeyPress += consoleKeyInfo =>
        {
            if (consoleKeyInfo.Key == ConsoleKey.Spacebar)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Hello!");
            }
        };
        
        Application.Window = mainWindow;
        Application.Start();
    }
}