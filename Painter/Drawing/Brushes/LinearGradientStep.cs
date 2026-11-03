namespace Painter.Drawing.Brushes;

public record LinearGradientStep(Color From, Color To, double Weight)
{
    private double? _previousWidth;
    private double? _stepR;
    private double? _stepG;
    private double? _stepB;
    
    public Color? GetColorAt(double x, double y, double width, double height)
    {
        if (width != _previousWidth)
        {
            _stepR = (To.R - From.R) / width;
            _stepG = (To.G - From.G) / width;
            _stepB = (To.B - From.B) / width;
            _previousWidth = width;
        }
        
        return new Color(
            (byte) (x * _stepR + From.R)!,
            (byte) (x * _stepG + From.G)!,
            (byte) (x * _stepB + From.B)!);
    }
}