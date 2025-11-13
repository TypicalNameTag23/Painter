using System.Diagnostics;
using System.Drawing;
using Painter;
using Painter.Drawing;
using Painter.Widgets;

namespace Sample;

public class FpsTacker : Widget
{
    private TextBlock _textBlock = new()
    {
        TextColor = Color.AliceBlue,
        BackgroundColor = Color.Black,
        TextAlignment = TextAlignment.Center,
        // MaxHeight = 10, todo: fix
        // MaxWidth = 30,
        // OverflowStyle = OverflowStyle.Cutoff,
        WrapText = true,
        // Width = 30,
        // Height = 5
    };
    
    private int _frames;
    private long _renderTime;
    private Stopwatch _renderStopwatch;
    private bool show;

    public bool Blinking { get; set; } = true;

    public FpsTacker()
    {
        _textBlock.Parent = this;
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
                    _textBlock.Text = $"FPS: {_frames} | RT: {_renderTime}ms" + " aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; // "asddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
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
                        _textBlock.Text = "";
                        show = false;
                        _frames = 0;
                    }
                }
                
                Thread.Sleep(1000);
            }
        }).Start();
    }
    
    public override void Draw(Canvas canvas)
    {
        _textBlock.Draw(canvas);
    }
}