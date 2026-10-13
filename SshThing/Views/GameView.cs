using Painter.Drawables;
using Painter.Drawing;
using Sample;
using SshThing.Drawables;

namespace SshThing.Views;

public class GameView : Drawable
{
    public GameView()
    {
        Attach(
            new Grid
                {
                    RowDefinitions = RowDefinition.Of("*, 1")
                }.SetContent(
                    new Panel
                        {
                            Content = [
                                new HsvColorSpaceVisualizer()
                                {
                                    Focusable = false
                                },
                                new Grid
                                {
                                    RowDefinitions = RowDefinition.Of("*, 11, *"),
                                    ColumnDefinitions = ColumnDefinition.Of("*, 25, *")
                                }.SetContent(new TicTacToeBoardDrawable(), row: 1, column: 1)
                            ]
                        }, row: 0)
                .SetContent(
                    new TextBlock
                    {
                        Text = "  [TAB]: Cycle Focus  |  [SPACE]: Interact  |  [B]: Exit Lobby  |  [CTRL+C]: Quit  ",
                        OverflowStyle = OverflowStyle.Cutoff,
                        BackgroundColor = Color.Black
                    }, row: 1)
        );   
    }
}