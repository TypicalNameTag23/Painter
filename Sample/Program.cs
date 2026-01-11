using Painter;
using Painter.Drawables;

namespace Sample;

static class Program
{
    static void Main()
    {
        var app = new Application
        {
            Content = new Panel
            {
                Content =
                [
                    new HsvColorSpaceVisualizer
                    {
                        FocusOrder = 4
                    },
                    new TextBox
                    {
                        Height = 2,
                        Width = 20,
                        X = 15,
                        Y = 30,
                        FocusOrder = 5
                    },
                    new Button
                    {
                        Height = 4,
                        Width = 10,
                        FocusOrder = 2,
                        Content = new Button
                        {
                            Width = 2,
                            Height = 2,
                            FocusOrder = 1,
                        }.OnClick((button, _) =>
                            {
                                button.Content = new TextBlock
                                {
                                    Text = "asd"
                                };
                            })
                    },
                    new Button
                    {
                        Width = 30,
                        Height = 2,
                        X = 10,
                        Y = 5,
                        FocusOrder = 3,
                        Content = new HsvColorSpaceVisualizer
                        {
                            Focusable = false,
                            Height = 20
                        }
                    }.OnClick((button, _) =>
                    {
                        ((HsvColorSpaceVisualizer)button.Content!).SaturationOffset += 0.01;
                    })
                ]
            }
        };
        
        app.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), app.Kill);
        app.Start();
    }
}


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