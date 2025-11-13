using System.Drawing;
using Painter;
using Painter.Widgets;

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

        Application.OnKeyPress += consoleKeyInfo =>
        {
            if (consoleKeyInfo.Key == ConsoleKey.Spacebar)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Pressed Space!");
            }
        };

        // Application.Drawable = new TextBlock
        // {
            // Text = "This is a big block of text that will show me how bad of a developer I am! This is soooo much fun!!!!!",
            // Width = 20,
            // Height = 2,
            // BackgroundColor = Color.IndianRed,
            // TextColor = Color.DarkRed,
            // WrapText = false
        // };

        Application.Drawable = new FpsTacker()
        {
            Blinking = false
        };
        
        Application.Start();
    }
}