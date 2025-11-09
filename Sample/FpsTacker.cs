using Taui;
using Taui.Composeables;
using Taui.Drawing;

namespace Sample;

public class FpsTacker : IComposable
{
    private TextBlock _textBlock = new();
    private int _frames;

    public FpsTacker()
    {
        Application.BeforeFrameRender += () =>
        {
            _frames += 1;
        };

        new Thread(_ =>
        {
            while (true)
            {
                _textBlock.Text = $"FPS: {_frames}";
                _frames = 0;
                Thread.Sleep(1000);
            }
        }).Start();
    }
    
    public void ComposeOnto(Canvas canvas)
    {
        _textBlock.ComposeOnto(canvas);
    }
}