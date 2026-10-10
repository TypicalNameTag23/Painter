using Painter;
using Painter.Drawables;
using Painter.Drawing;
using Painter.Drawing.Brushes;
using Sample.ViewModels;

namespace Sample;

static class Program
{
    static void Main()
    {
        var grid = new Grid
        {
            RowDefinitions = RowDefinition.Of("2*, 3*, 5*"),
            ColumnDefinitions = ColumnDefinition.Of("12, 10*, *")
        };

        var viewModel = new MainWindowViewModel();
        // PopulateGrid(grid, viewModel);
        var app = new Application
        {
            Content = 
                new Grid
                    {
                        RowDefinitions = RowDefinition.Of("1, *"),
                    }
                    .SetContent(new FpsTacker(), row: 0)
                    .SetContent(
                        new Grid
                            {
                                ColumnDefinitions = ColumnDefinition.Of("10*, 20, *")
                            }
                            .SetContent(
                                new Rectangle 
                                { 
                                    Margin = Thickness.Of(top: 1, left: 2, right: 2, bottom: 2), 
                                    BackgroundColor = new LinearGradientColorBrush(
                                        (Color.Crimson, 6),
                                        (Color.MediumVioletRed, 2), 
                                        (Color.Blue, 5),
                                        (Color.DeepPink, 7),
                                        (Color.White, 4),
                                        (Color.DarkSalmon, 0)) 
                                }, column: 0)
                            .SetContent(new HsvColorSpaceVisualizer(), column: 1)
                            .SetContent(
                                new Rectangle 
                                { 
                                    Margin = Thickness.Of(top: 1, left: 2, right: 2, bottom: 2), 
                                    BackgroundColor = new LinearGradientColorBrush(
                                        (Color.Crimson, 6),
                                        (Color.MediumVioletRed, 2), 
                                        (Color.Blue, 5),
                                        (Color.DeepPink, 7),
                                        (Color.White, 4),
                                        (Color.DarkSalmon, 0)) 
                                }, column: 2)
                        , row: 1)
        };
        
        app.AddGlobalKeyBind(new Key(ConsoleKey.C, 'c', ConsoleModifiers.Control), (_, _) => app.Kill());
        app.Start();
    }

    // private static void PopulateGrid(Grid grid, MainWindowViewModel viewModel)
    // {
    //     for (var row = 0; row < grid.RowDefinitions.Count; row++)
    //     {
    //         for (var column = 0; column < grid.ColumnDefinitions.Count; column++)
    //         {
    //             var index = row + column * grid.RowDefinitions.Count;
    //             grid.SetContent(row, column, new HsvColorSpaceVisualizer
    //             {
    //                 HueOffset = index % 359,
    //                 // SaturationOffset = index / 100d % 1,
    //                 FocusOrder = index
    //             });
    //         }
    //     }
    //     
    //     grid/*.SetContent(1, 0, new HsvColorSpaceVisualizer
    //         {
    //             FocusOrder = 2
    //         })*/
    //         .SetContent(1, 1, new Button
    //         {
    //             FocusOrder = 1,
    //             Content = new TextBlock
    //                 {
    //                     DataContext = viewModel
    //                 }
    //                 .Bind("BackgroundColor", nameof(viewModel.BackgroundColor), BindingType.TwoWay)
    //                 .Bind("Text", nameof(viewModel.ButtonClickTimes), BindingType.TwoWay)
    //         }.OnClick(viewModel.OnClick));
    // }
    
}