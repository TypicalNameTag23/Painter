using System.Drawing;
using Painter.Drawables;
using Painter.Events;

namespace Sample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public int ButtonClickTimes
    {
        get;
        set
        {
            if (SetPropertyAndNotifyIfChanged(ref field, value))
            {
                OnPropertyChangedEvent(nameof(BackgroundColor));
            }
        }
    }

    public Color BackgroundColor => ColorHelper.FromHsv(ButtonClickTimes % 359, 0.8, 0.8);

    public void OnClick(Button sender, ButtonClickedEventArgs args)
    {
        ButtonClickTimes++;
    }
}