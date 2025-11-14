using System.Drawing;

namespace Painter.Drawing;

//todo: buffered frame alongside previous frame
//todo: remove old pixels that have not been set
//todo: console abstraction that allows for printing out frame
//todo: console abstraction that manages platform specific implementation details
public class FrameCache
{
    private (char character, Color? foregroundColor, Color? backgroundColor)?[] _cachedFrame;
    private int _width;
    private int _height;
    private int _frameArrayLength;
    
    public FrameCache(int width, int height)
    {
        _width = width;
        _height = height;
        _frameArrayLength = width * height;
        _cachedFrame = new(char character, Color? foregroundColor, Color? backgroundColor)?[_frameArrayLength];
    }

    public (char character, Color? foregroundColor, Color? backgroundColor)? GetCachedPixel(int x, int y)
    {
        var pixelIndex = x + y * _width;
        return pixelIndex >= _frameArrayLength ? null : _cachedFrame[pixelIndex];
    }

    public void SetCachedPixel(int x, int y, char character, Color? foregroundColor, Color? backgroundColor)
    {
        var pixelIndex = x + y * _width;
        if (pixelIndex >= _frameArrayLength)
        {
            return;
        }

        _cachedFrame[pixelIndex] = (character, foregroundColor, backgroundColor);
    }

    public void Resize(int width, int height)
    {
        _width = width;
        _height = height;
        
        var frameArrayLength = width * height;
        if (frameArrayLength > _frameArrayLength)
        {
            _cachedFrame = new(char character, Color? foregroundColor, Color? backgroundColor)?[frameArrayLength];
        }

        _frameArrayLength = frameArrayLength;
    }
}