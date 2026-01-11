using System.Drawing;
using Painter;
using Painter.Drawables;

namespace Sample;

static class Program
{
    static void Main()
    {
        // var app = new Application
        // {
        //     Content = new Panel
        //     {
        //         Content =
        //         [
        //             new HsvColorSpaceVisualizer
        //             {
        //                 FocusOrder = 4
        //             },
        //             new TextBox
        //             {
        //                 Height = 2,
        //                 Width = 20,
        //                 X = 15,
        //                 Y = 30,
        //                 FocusOrder = 5
        //             },
        //             new Button
        //             {
        //                 Height = 4,
        //                 Width = 10,
        //                 FocusOrder = 2,
        //                 Content = new Button
        //                 {
        //                     Width = 2,
        //                     Height = 2,
        //                     FocusOrder = 1,
        //                 }.OnClick((button, _) =>
        //                     {
        //                         button.Content = new TextBlock
        //                         {
        //                             Text = "asd"
        //                         };
        //                     })
        //             },
        //             new Button
        //             {
        //                 Width = 30,
        //                 Height = 2,
        //                 X = 10,
        //                 Y = 5,
        //                 FocusOrder = 3,
        //                 Content = new HsvColorSpaceVisualizer
        //                 {
        //                     Focusable = false,
        //                     Height = 20
        //                 }
        //             }.OnClick((button, _) =>
        //             {
        //                 ((HsvColorSpaceVisualizer)button.Content!).SaturationOffset += 0.01;
        //             })
        //         ]
        //     }
        // };

        var colors = new List<Color>
        {
            Color.Coral, Color.Brown, Color.DarkBlue,
            Color.RebeccaPurple, Color.DarkGreen, Color.DodgerBlue,
            Color.BlueViolet, Color.Crimson, Color.Fuchsia,
        };

        var grid = new Grid
        {
            RowDefinitions = RowDefinition.Of("2*, 20, 4*"),
            ColumnDefinitions = ColumnDefinition.Of("3*, 1*, 100")
        };

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                grid.SetContent(i, j, new TextBlock
                {
                    Text = $"[Row: {i} | Column: {j}]",
                    BackgroundColor = colors[i * 3 + j]
                });
            }
        }

        var x = 0;
        grid.SetContent(1, 0, new HsvColorSpaceVisualizer
            {
                FocusOrder = 2
            })
            .SetContent(1, 1, new Button 
            {
                FocusOrder = 1
            }.OnClick((button, _) =>
            {
                x++;
                button.Content ??= new TextBlock();
                ((TextBlock)button.Content!).BackgroundColor = colors[x % colors.Count];
                ((TextBlock)button.Content!).Text = x.ToString();
            }))
            .SetContent(2, 2, new TextBox());
        
        var app = new Application
        {
            Content = grid
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