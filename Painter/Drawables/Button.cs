using Painter.Drawing;
using Painter.Drawing.Canvas;
using Painter.Events;

namespace Painter.Drawables;

public class Button : ContentControl
{
    public override bool Focusable { get; set; } = true;
    public bool Selected { get; private set; } = false;

    public Color FocusedColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = Color.White;

    public Color UnfocusedColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = Color.Black;

    public Color DisabledColor
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    } = Color.Black;

    public event EventHandler<ButtonClickedEventArgs>? OnClickEvent;

    public Button OnClick(EventHandler<ButtonClickedEventArgs> onClick)
    {
        OnClickEvent += onClick;
        return this;
    }

    // public override void OnFocusChanged(bool focused)
    // {
    //     BackgroundColor = focused ? FocusedColor : UnfocusedColor;
    // }

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
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                canvas.Draw(x + X, y + Y, new StrokeStyle
                {
                    BackgroundColor = GetBackgroundColor()
                });
            }
        }
        
        base.Draw(canvas);
    }

    private Color GetBackgroundColor()
    {
        if (!Focusable)
        {
            return DisabledColor;
        }
        
        return Focused ? FocusedColor : UnfocusedColor;
    }
}