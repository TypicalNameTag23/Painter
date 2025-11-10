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
            Composable = new FpsTacker
            {
                Blinking = true
            }
        };

        Application.OnKeyPress += consoleKeyInfo =>
        {
            if (consoleKeyInfo.Key == ConsoleKey.Spacebar)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0");
            }
        };
        
        Application.Window = mainWindow;
        Application.Start();
    }
}