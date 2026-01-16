using Painter.Drawables;
using Painter.Drawing.Canvas;
using Painter.Platform;

namespace Painter.Rendering;

public class Renderer : IRenderer, ILayoutManager
{
    private readonly TerminalCanvas _canvas;
    private readonly Terminal _terminal;
    private readonly TopLevel _root;
    
    internal Renderer(Terminal terminal, TopLevel root)
    {
        _terminal = terminal;
        _root = root;
        _canvas = new TerminalCanvas(_terminal);
        _terminal.TerminalSizeChangedEvent += (_, args) =>
        {
            Console.Write(AnsiTerminalCodes.ClearScreen);
            root.Width = args.Width;
            root.Height = args.Height;
        };
    }

    private List<Drawable> _drawablesNeedingRendering = [];
    private List<Drawable> _drawablesNeedingMeasuring = [];
    private List<Drawable> _drawablesNeedingArranging = [];

    public void InvalidateRender(Drawable drawable)
    {
        _drawablesNeedingRendering.Add(drawable);
    }

    public void InvalidateArrange(Drawable drawable)
    {
        _drawablesNeedingArranging.Add(drawable);
    }

    public void InvalidateMeasure(Drawable drawable)
    {
        _drawablesNeedingMeasuring.Add(drawable);
    }
    
    public void ExecuteRenderPass()
    {
        if (_drawablesNeedingRendering.Count == 0)
        {
            return;
        }
        
        if (_drawablesNeedingArranging.Count > 0 || _drawablesNeedingMeasuring.Count > 0)
        {
            ExecuteLayoutPass();
        }
        
        _root.Draw(_canvas);
    }

    public void ExecuteLayoutPass()
    {
        throw new NotImplementedException();
    }
}