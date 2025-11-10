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
            Composable = new FpsTacker()
        };

        app.Window = mainWindow;
        app.Start();
    }
}