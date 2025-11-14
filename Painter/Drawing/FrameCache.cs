using System.Drawing;

namespace Painter.Drawing;

//todo: buffered frame alongside previous frame
//todo: remove old pixels that have not been set
//todo: console abstraction that allows for printing out frame
//todo: console abstraction that manages platform specific implementation details
public class FrameCache
{
    private (char character, Style style)?[] _cachedFrame;
    private int _width;
    private int _height;
    private int _frameArrayLength;
    
    public FrameCache(int width, int height)
    {
        _width = width;
        _height = height;
        _frameArrayLength = width * height;
        _cachedFrame = new(char character, Style style)?[_frameArrayLength];
    }

    public bool CachedPixelMatches(int x, int y, char character, Style style)
    {
        var pixel = GetCachedPixel(x, y);
        if (pixel is null)
        { // todo: make better
            // return character == ' ' && 
                   // style.ForegroundColor is null &&
                   // style.BackgroundColor is null &&
                   // style is { Bold: false, Italic: false, Strikethrough: false, Underline: false };
        }

        return pixel.Value.character == character && pixel.Value.style == style;
    }
    
    public (char character, Style style)? GetCachedPixel(int x, int y)
    {
        if (x > _width || y > _height)
        {
            return null;
        }
        
        return _cachedFrame[x + y * _width];
    }
    

    public void SetCachedPixel(int x, int y, char character, Style style)
    {
        if (x > _width || y > _height)
        {
            return;
        }

        _cachedFrame[x + y * _width] = (character, style);
    }

    public void Resize(int width, int height)
    {
        _width = width;
        _height = height;
        
        var frameArrayLength = width * height;
        if (frameArrayLength > _frameArrayLength)
        {
            _cachedFrame = new(char character, Style style)?[frameArrayLength];
        }
        else
        {
            Array.Clear(_cachedFrame);
        }

        _frameArrayLength = frameArrayLength;
    }
}