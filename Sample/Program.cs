using System.Drawing;
using Painter;
using Painter.Elements;
using Painter.Layouts;

namespace Sample;

static class Program
{
    static void Main()
    {
        var app = new Application
        {
            Drawable = new Panel
            {
                Elements = [
                    // new TextBlock 
                    // {
                        // Text = "This is a big block of text that will show me how bad of a developer I am! This is soooo much fun!!!!!",
                        // Y = 25,
                        // Width = 20, Height = 2, 
                        // TextColor = Color.DarkRed, 
                        // BackgroundColor = Color.IndianRed, 
                        // WrapText = false
                    // },
                    // new TextBlock
                    // {
                        // Text = "This another GIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANTTTTTTT BLOOOOOOOOOOOOOOCK OOOOOOOOOOFFFFFFFFFFFF TEEEEEEEEEEEEEEEEXXXXXXXXXXXXTTTTTTTTTTT!!!!!!!!!!!!!!!a",
                        // X = 5, Y = 5,
                        // Width = 30, 
                        // MaxHeight = 8,
                        // TextColor = Color.CadetBlue,
                        // BackgroundColor = Color.Cornsilk 
                    // },
                    // new TextBlock
                    // {
                        // Text = "When will Infurion upload his next video :thinking:",
                        // X = 20, Y = 20,
                        // MaxHeight = 8, MaxWidth = 40,
                        // TextColor = Color.FloralWhite,
                        // BackgroundColor = Color.Coral,
                        // TextAlignment = TextAlignment.Center
                    // },
                    // new TextBlock
                    // {
                        // Text = "this text box is cut off to the right side of the screen lol", 
                        // X = -10, Y = 3, 
                        // MaxHeight = 8, MaxWidth = 40,
                        // TextColor = Color.FloralWhite,
                        // BackgroundColor = Color.FromArgb(100, 60, 250),
                        // TextAlignment = TextAlignment.Left
                    // },
                    // new TextBlock
                    // {
                        // Text = "and this one is cut off on the right side of the screen",
                        // X = 70, Y = 3, MaxHeight = 8, Width = 40,
                        // TextColor = Color.FloralWhite,
                        // BackgroundColor = Color.FromArgb(100, 60, 150),
                        // TextAlignment = TextAlignment.Right
                    // },
                    new HsvColorSpaceVisualizer()
                ]
            }
        };
        
        app.AddGlobalKeyBind(new Key(ConsoleKey.Escape), app.Kill);
        app.Start();
    }
}