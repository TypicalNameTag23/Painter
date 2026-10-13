using System.Runtime.InteropServices.ComTypes;
using Painter.Drawables;
using Painter.Drawing;
using Sample;

namespace SshThing.Views;

public class JoinLobbyView : Drawable
{
    public JoinLobbyView(Action joinButtonClick)
    {
        var joinButton = new Button
        {
            FocusedColor = new Color(53, 53, 53),
            Content = new TextBlock
            {
                Text = "Join",
                Alignment = Alignment.Center
            }
        };
        joinButton.OnClickEvent += (_, _) => joinButtonClick();
        
        Attach(
            new Grid
                {
                    RowDefinitions = RowDefinition.Of("*, 1")
                }.SetContent(
                    new Grid
                    {
                        RowDefinitions = RowDefinition.Of("*, 3, 2, 1, *"),
                        ColumnDefinitions = ColumnDefinition.Of("*, 1.5*, *")
                    }
                    .SetContent(new Panel
                    {
                        Height = 3,
                        Content = [
                            new HsvColorSpaceVisualizer
                            {
                                SaturationOffset = 0.5,
                                Height = 6,
                                Value = 0.8,
                                Focusable = false
                            },
                            new TextBlock
                            {
                                Text = "Enter Lobby Code",
                                Alignment = Alignment.Center,
                                Height = 1,
                                Y = 1
                            }
                        ]
                    }, row: 1, column: 1)
                    .SetContent(
                        new Grid
                        {
                            ColumnDefinitions =  ColumnDefinition.Of("*, 1, 8")
                        }
                        .SetContent(new TextBox(), column: 0)
                        .SetContent(joinButton, column: 2), row: 3, column: 1), row: 0)
                .SetContent(
                    new TextBlock
                    {
                        Text = "  [TAB]: Cycle Focus  |  [SPACE]: Interact  |  [B]: Previous Page  |  [CTRL+C]: Quit  ",
                        OverflowStyle = OverflowStyle.Cutoff,
                        BackgroundColor = Color.Black
                    }, row: 1)
        );   
    }
    
    ~JoinLobbyView()
    {
        Console.WriteLine("JoinLobbyView destroyed.");    
    }
}