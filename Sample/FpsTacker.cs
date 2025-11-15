using System.Diagnostics;
using System.Drawing;
using Painter;
using Painter.Drawing;
using Painter.Elements;

namespace Sample;

public sealed class FpsTacker : Element
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
        // Width = null,
        // Height = 3
    };
    
    private int _frames;
    private double _renderTime;
    private double _maxRenderTime;
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
            _renderTime = _renderStopwatch.Elapsed.TotalMilliseconds;
            if (_renderTime > _maxRenderTime)
            {
                _maxRenderTime = _renderTime;
            }
        };

        new Thread(_ =>
        {
            while (true)
            {
                if (!Blinking)
                {
                    Redraw();
                    show = true;
                    _textBlock.Text = $"FPS: {_frames} | RT: {(_renderTime):N4}ms | MRT: {(_maxRenderTime):N4}ms";
                    _frames = 0;
                }
                else
                {
                    if (!show)
                    {
                        Redraw();
                        _textBlock.Text = $"FPS: {_frames} | RT: {_renderTime * 0.000001}ms | MRT: {_maxRenderTime * 0.000001}ms";
                        _frames = 0;
                        show = true;
                    }
                    else
                    {
                        Redraw();
                        _textBlock.Text = "";
                        show = false;
                        _frames = 0;
                    }
                }
                
                Thread.Sleep(1000);
            }
        }).Start();
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        _textBlock.Draw(canvas, force);
    }
}