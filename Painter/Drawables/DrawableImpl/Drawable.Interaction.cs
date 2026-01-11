using System.Diagnostics;
using Painter.Events;

namespace Painter.Drawables.DrawableImpl;

public abstract partial class Drawable
{
    public virtual bool Focusable { get; set; }

    public uint FocusOrder { get; set; }

    public virtual bool Focused
    {
        get;
        set => SetAndRedrawIfChanged(ref field, value);
    }

    public virtual void Interact(KeyPressedEventArgs args) { }

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

    internal void CycleFocus(CycleDirection cycleDirection)
    {
        var focused = GetFocusedChild();
        var focusableChildren = GetFocusableChildren();
        if (focused is null)
        {
            if (focusableChildren.Count == 0)
            {
                return;
            }
            
            switch (cycleDirection)
            {
                case CycleDirection.Next:
                {
                    focusableChildren[0].Focused = true;
                    break;
                }
                case CycleDirection.Previous:
                {
                    focusableChildren[^1].Focused = true;
                    break;
                }
            }
            
            return;
        }

        
        focused.Focused = false;
        switch (cycleDirection)
        {
            case CycleDirection.Next:
            {
                var nextIndex = focusableChildren.IndexOf(focused) + 1;
                if (nextIndex == focusableChildren.Count)
                {
                    return;
                }
                
                focusableChildren[nextIndex].Focused = true;
                break;
            }
            case CycleDirection.Previous:
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