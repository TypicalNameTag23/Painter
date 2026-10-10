using Painter.Drawing;
using Painter.Drawing.Canvas;
using Painter.Events;

namespace Painter.Drawables;

public class Button : ContentControl
{
    public override bool Focusable { get; set; } = true;
    
    public bool Selected { get; private set; } = false;

    public Color BackgroundColor { get; set; } = Color.Black;

    public Color FocusedColor { get; set; } = Color.Black;

    public Color SelectedColor { get; set; } = Color.White;

    public event EventHandler<ButtonClickedEventArgs>? OnClickEvent;

    public Button OnClick(EventHandler<ButtonClickedEventArgs> onClick)
    {
        OnClickEvent += onClick;
        return this;
    }
    
    public override void Interact(KeyPressedEventArgs args)
    {
        if (args.Key.KeyType != ConsoleKey.Spacebar)
        {
            return;
        }
        
        OnClickEvent?.Invoke(this, new ButtonClickedEventArgs(this));
    }

    public override void Draw(ICanvas canvas)
    {
        var backgroundColor = Focused ? FocusedColor : BackgroundColor;
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                canvas.Draw(x + X, y + Y, new StrokeStyle
                {
                    BackgroundColor = backgroundColor
                });
            }
        }
        
        base.Draw(canvas);
    }
}