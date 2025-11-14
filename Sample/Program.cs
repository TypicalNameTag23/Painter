using System.Drawing;
using Painter;
using Painter.Elements;
using Painter.Layouts;

namespace Sample;

static class Program
{
    static void Main()
    {
        var panel = new Panel();
        panel.Add([
            new TextBlock
            {
                Text = "This is a big block of text that will show me how bad of a developer I am! This is soooo much fun!!!!!",
                Width = 20,
                Height = 2,
                BackgroundColor = Color.IndianRed,
                TextColor = Color.DarkRed,
                WrapText = false
            },
            new TextBlock
            {
                Text = "This another GIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANTTTTTTT BLOOOOOOOOOOOOOOCK OOOOOOOOOOFFFFFFFFFFFF TEEEEEEEEEEEEEEEEXXXXXXXXXXXXTTTTTTTTTTT!!!!!!!!!!!!!!!a",
                Width = 30,
                Height = null,
                X = 5,
                Y = 5,
                MaxHeight = 8,
                BackgroundColor = Color.Cornsilk,
                TextColor = Color.CadetBlue,
                WrapText = true,
                ShowDebugInfo = true
            },
            new TextBlock
            {
                Text = "When will Infurion upload his next video :thinking:",
                Width = null,
                Height = null,
                X = 20,
                Y = 20,
                MaxHeight = 8,
                MaxWidth = 40,
                BackgroundColor = Color.Coral,
                TextColor = Color.FloralWhite,
                TextAlignment = TextAlignment.Center,
                WrapText = true
            },
            new FpsTacker
            {
                Blinking = false
            }
        ]);
        
        var app = new Application
        {
            TargetFramerate = -1,
            Drawable = panel
        };
        
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

        // Application.Drawable = 
        
        app.Start();
    }
}