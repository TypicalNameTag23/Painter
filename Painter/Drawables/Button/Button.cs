using System.Drawing;
using Painter.Drawables.DrawableImpl;
using Painter.Drawing;
using Painter.Events;

namespace Painter.Drawables.Button;

public class Button : Drawable
{
    private readonly ContentControl _contentControl = new();

    public Button()
    {
        Attach(_contentControl);
    }
    
    public override bool Focusable { get; set; } = true;
    
    public bool Selected { get; private set; } = false;

    public Color BackgroundColor { get; set; } = Color.Black;

    public Color FocusedColor { get; set; } = Color.Gray;

    public Color SelectedColor { get; set; } = Color.CadetBlue;

    public event EventHandler<Button, ButtonClickedEventArgs>? OnClickEvent;

    public Drawable? Content
    {
        get => _contentControl.Content;
        set => _contentControl.Content = value;
    }

    public Button OnClick(EventHandler<Button, ButtonClickedEventArgs> onClick)
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
        
        OnClickEvent?.Invoke(this, new ButtonClickedEventArgs());
    }

    protected override void DrawImpl(Canvas canvas)
    {
        var backgroundColor = Focused ? FocusedColor : BackgroundColor;
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                canvas.Draw(x + X, y + Y, new Style()
                {
                    BackgroundColor = backgroundColor
                });
            }
        }
        
        _contentControl.Draw(canvas);
    }
}