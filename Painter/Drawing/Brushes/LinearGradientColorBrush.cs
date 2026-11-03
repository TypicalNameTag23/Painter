namespace Painter.Drawing.Brushes;

public record LinearGradientColorBrush : IBrush
{
    // Is this really the only way I can think of doing this?
    // How does this translate to angled / non-linear gradients?
    // Probably redo this and make it like Avalonia where you give an offset out of 1 instead of a weight
    private readonly LinearGradientStep[] _steps;
    private readonly double[] _stepPercents;
    private readonly double[] _stepWidths;
    private int _previousWidth;
    
    public LinearGradientColorBrush(params Color[] colors)
    {
        if (colors.Length == 0)
        {
            return;
        }
        
        var steps = colors.Length - 1;
        _steps = new LinearGradientStep[steps];
        _stepPercents = new double[steps];
        _stepWidths = new double[steps];
        for (var i = 0; i < colors.Length - 1; i++)
        {
            _steps[i] = new LinearGradientStep(colors[i], colors[i + 1], 1);
            _stepPercents[i] = _steps[i].Weight / steps;
        }
    }
    
    // todo: probably create a LinearGradientColorBrushFactory?
    public LinearGradientColorBrush(params (Color, double)[] colorsAndWeights)
    {
        if (colorsAndWeights.Length == 0)
        {
            return;
        }
        
        var totalWeight = colorsAndWeights.Sum(step => step.Item2) - colorsAndWeights[^1].Item2;
        
        _steps = new LinearGradientStep[colorsAndWeights.Length - 1];
        _stepPercents = new double[colorsAndWeights.Length - 1];
        _stepWidths = new double[colorsAndWeights.Length - 1];
        for (var i = 0; i < colorsAndWeights.Length - 1; i++)
        {
            _steps[i] = new LinearGradientStep(colorsAndWeights[i].Item1, colorsAndWeights[i + 1].Item1, colorsAndWeights[i].Item2);
            _stepPercents[i] = _steps[i].Weight / totalWeight;
        }
    }
    
    // Actually, don't expose the LinearGradientStep for setting, since it will allow for creating jumps between the steps
    public LinearGradientColorBrush(params LinearGradientStep[] steps)
    {
        if (steps.Length == 0)
        {
            return;
        }

        var totalWeight = steps.Sum(step => step.Weight);
        _steps = steps;
        _stepPercents = new double[steps.Length];
        _stepWidths = new double[steps.Length];
        for (var i = 0; i < steps.Length - 1; i++)
        {
            _stepPercents[i] = _steps[i].Weight / totalWeight;
        }
    }

    public Color? GetColorAt(int x, int y, int width, int height)
    {
        if (_steps.Length == 0)
        {
            return Color.Transparent;
        }
        
        if (_previousWidth != width)
        {
            MeasureStepWidths(width);
            _previousWidth = width;
        }

        int? step = null;
        var usedWidth = 0d;
        for (var i = 0; i < _steps.Length; i++)
        {
            usedWidth += _stepWidths[i];
            if (usedWidth > x)
            {
                step = i;
                break;
            }
        }

        // There is a bug if the step is still null, allow it to be seen by just allowing the exception to be thrown
        var color = _steps[step!.Value].GetColorAt(x - usedWidth + _stepWidths[step.Value], y, _stepWidths[step.Value], height);
        return color;
    }
    
    private void MeasureStepWidths(int totalWidth)
    {
        var remainingWidth = (double) totalWidth;
        for (var i = 0; i < _steps.Length; i++)
        {
            if (i == _steps.Length - 1)
            {
                _stepWidths[i] = remainingWidth;
                return;
            }
            
            var stepWidth = totalWidth * _stepPercents[i];
            _stepWidths[i] = stepWidth;
            remainingWidth -= stepWidth;
        }
    }
}