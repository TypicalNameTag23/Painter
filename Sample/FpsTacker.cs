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
        // MaxHeight = 10,
        // MaxWidth = 30,
        // OverflowStyle = OverflowStyle.Cutoff,
        WrapText = true,
        // Width = null,
        // Height = 3
    };
    
    private int _frames;
    private double _renderTime;
    private double _maxRenderTime;
#if DEBUG
    private double _drawTime;
    private double _maxDrawTime;
    private Stopwatch _drawStopwatch;
    private double _drawC;
    private double _maxDrawC;
    private Stopwatch _drawCStopwatch;
#endif
    private Stopwatch _renderStopwatch;

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

#if DEBUG
        Application.BeforeDraw += () =>
        {
            _drawStopwatch = Stopwatch.StartNew();
        };
        Application.AfterDraw += () =>
        {
            _drawStopwatch!.Stop();
            _drawTime = _drawStopwatch.Elapsed.TotalMilliseconds;
            if (_drawTime > _maxDrawTime)
            {
                _maxDrawTime = _drawTime;
            }
        };
        Canvas.DrawStart += () =>
        {
            _drawCStopwatch = Stopwatch.StartNew();
        };
        Canvas.DrawEnd += () =>
        {
            _drawCStopwatch!.Stop();
            _drawC = _drawCStopwatch.Elapsed.TotalMilliseconds;
            if (_drawC > _maxDrawC)
            {
                _maxDrawC = _drawC;
            }
        };
#endif

        new Thread(_ =>
        {
            while (true)
            {
                Redraw();
                _textBlock.Text = $"FPS: {_frames} | RT: {(_renderTime):N4}ms | MRT: {(_maxRenderTime):N4}ms";
#if DEBUG
                _textBlock.Text += $" | DT: {_drawTime}ms | MDT: {_maxDrawTime}ms | PT: {_renderTime - _drawTime:N4} | CD: {_drawC:N4}ms | MCD: {_maxDrawC:N4}ms";
#endif
                _frames = 0;
                Thread.Sleep(1000);
            }
        }).Start();
    }
    
    protected override void DrawElement(Canvas canvas, bool force)
    {
        _textBlock.Draw(canvas, force);
    }
}