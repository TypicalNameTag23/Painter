using Painter;
using Painter.Components;

namespace Sample;

static class Program
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
            Drawable = new TextBlock
            {
                Text = "asddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd98712 ny398yn981732y 48713y7981y379813 END"
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