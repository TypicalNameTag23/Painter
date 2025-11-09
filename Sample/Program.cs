using System.Runtime.InteropServices.ComTypes;
using Taui;
using Taui.Composeables;

namespace Sample;

class Program
{
    static void Main()
    {
        var app = new Application();
        app.TargetFps = -1;
        app.OnQuitKeyPress += (_, args) =>
        {
            args.Cancel = false;
        };
        
        var mainWindow = new Window
        {
            Composable = new FpsTacker()/*new TextBlock
            {
                Text = $"{app.Width} | {app.Height}",
                TextColor = ConsoleColor.Red,
                BackgroundColor = ConsoleColor.DarkGray
            }*/
        };

        app.Window = mainWindow;
        app.Start();
    }
}