using Painter.Drawing;

namespace Painter.Utilities;

public static class StringExtensions
{
    /// <summary>
    /// Gets the substring safely
    /// </summary>
    /// <param name="text">The string to substring</param>
    /// <param name="from">The starting index, will return <see cref="String.Empty"/> if it is greater than the length of the string</param>
    /// <param name="to">The ending index (exclusive), will return up to the end of the string if greater than the length of the string</param>
    /// <returns></returns>
    public static string SubstringSafely(this string text, int from, int to)
    {
        if (from > text.Length)
        {
            return string.Empty;
        }
        
        return text[from..(to > text.Length ? text.Length : to)];
    }
    
    /// <summary>
    /// Aligns a string given a width
    /// </summary>
    /// <param name="text"></param>
    /// <param name="width"></param>
    /// <param name="alignment"></param>
    /// <returns></returns>
    public static string Align(this string text, int width, Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.Right:
            {
                return text.PadLeft(width, ' ');
            }

            case Alignment.Center:
            {
                var leftPadding = width / 2 + text.Length / 2;
                text = text.PadLeft(leftPadding, ' ');
                text = text.PadRight(width, ' ');
                break;
            }

            case Alignment.Left:
            {
                return text.PadRight(width, ' ');
            }
        }

        return text;
    }
}