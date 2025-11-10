using System.Diagnostics;
using System.Numerics;
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
    private bool show = false;

    public bool Blinking { get; set; } = true;

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
                if (!Blinking)
                {
                    show = true;
                    _textBlock.Text = $"FPS: {_frames} | RT: {_renderTime}ms";
                    _frames = 0;
                }
                else
                {
                    if (!show)
                    {
                        _textBlock.Text = $"FPS: {_frames} | RT: {_renderTime}ms";
                        _frames = 0;
                        show = true;
                    }
                    else
                    {
                        show = false;
                        _frames = 0;
                    }
                }
                
                
                
                Thread.Sleep(1000);
            }
        }).Start();
    }
    
    public void ComposeOnto(Canvas canvas)
    {
        if (show)
        {
            _textBlock.ComposeOnto(canvas);
        }
    }
}