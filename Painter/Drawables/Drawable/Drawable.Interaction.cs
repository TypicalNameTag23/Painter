using Painter.Drawables.DrawableImpl;
using Painter.Events;

namespace Painter.Drawables;

public abstract partial class Drawable
{
    public virtual bool Focusable { get; set; }

    public int FocusOrder { get; set; } = int.MaxValue;

    public virtual bool Focused
    {
        get;
        set
        {
            if (Focused != value)
            {
                OnFocusChanged(value);
            }
            
            SetAndRedrawIfChanged(ref field, value);
        }
    }

    public virtual void Interact(KeyPressedEventArgs args) { }
    
    public virtual void OnFocusChanged(bool focused) { }

    internal Drawable? GetFocusedChild()
    {
        foreach (var drawable in Children)
        {
            if (drawable.Focused)
            {
                return drawable;
            }

            var focused = drawable.GetFocusedChild();
            if (focused is not null)
            {
                return focused;
            }
        }
        
        return null;
    }

    internal void CycleFocus(Direction direction)
    {
        var focused = GetFocusedChild();
        var focusableChildren = GetFocusableChildren();
        if (focused is null)
        {
            if (focusableChildren.Count == 0)
            {
                return;
            }
            
            switch (direction)
            {
                case Direction.Backward:
                {
                    focusableChildren[0].Focused = true;
                    break;
                }
                case Direction.Forward:
                {
                    focusableChildren[^1].Focused = true;
                    break;
                }
            }
            
            return;
        }

        
        focused.Focused = false;
        switch (direction)
        {
            case Direction.Backward:
            {
                var nextIndex = focusableChildren.IndexOf(focused) + 1;
                if (nextIndex == focusableChildren.Count)
                {
                    return;
                }
                
                focusableChildren[nextIndex].Focused = true;
                break;
            }
            case Direction.Forward:
            {
                var nextIndex = focusableChildren.IndexOf(focused) - 1;
                if (nextIndex == -1)
                {
                    return;
                }
                
                focusableChildren[nextIndex].Focused = true;
                break;
            }
        }
    }
    
    internal void LoseFocus()
    {
        GetFocusedChild()?.Focused = false;
    }
}