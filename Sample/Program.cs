using System.Drawing;
using Painter;
using Painter.Data;
using Painter.Drawables;
using Sample.ViewModels;

namespace Sample;

static class Program
{
    static void Main()
    {
        var grid = new Grid
        {
            RowDefinitions = RowDefinition.Of("*, *, *"),
            ColumnDefinitions = ColumnDefinition.Of("*, *, *")
        };

        var viewModel = new MainWindowViewModel();
        PopulateGrid(grid, viewModel);
        var app = new Application
        {
            Content = grid,
            DataContext = viewModel
        };
        
        app.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), (_, _) => app.Kill());
        app.Start();
    }

    private static void PopulateGrid(Grid grid, MainWindowViewModel viewModel)
    {
        var colors = new List<Color>
        {
            Color.Coral, Color.Brown, Color.DarkBlue,
            Color.RebeccaPurple, Color.DarkGreen, Color.DodgerBlue,
            Color.BlueViolet, Color.Crimson, Color.Fuchsia,
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
        
        grid.SetContent(1, 0, new HsvColorSpaceVisualizer
            {
                FocusOrder = 2
            })
            .SetContent(1, 1, new Button
            {
                FocusOrder = 1,
                Content = new TextBlock
                    {
                        DataContext = viewModel
                    }
                    .Bind("BackgroundColor", nameof(viewModel.BackgroundColor), BindingType.TwoWay)
                    .Bind("Text", nameof(viewModel.ButtonClickTimes), BindingType.TwoWay)
            }.OnClick(viewModel.OnClick));
    }
    
}