using System.Diagnostics;
using Taui;
using Taui.Composeables;
using Taui.Drawing;

namespace Sample;

public class FpsTacker : IComposable
{
    private TextBlock _textBlock = new();
    private int _frames;
    private long _renderTime;
    private Stopwatch _renderStopwatch;

    public FpsTacker()
    {
        Application.BeforeFrameRender += () =>
        {
            _renderStopwatch = Stopwatch.StartNew();
        };
        
        Application.AfterFrameRender += () =>
        {
            _frames += 1;
            _renderStopwatch!.Stop();
            _renderTime = _renderStopwatch.ElapsedMilliseconds;
        };

        new Thread(_ =>
        {
            while (true)
            {
                _textBlock.Text = $"FPS: {_frames} | RT: {_renderTime}ms";
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