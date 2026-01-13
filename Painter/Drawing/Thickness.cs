namespace Painter.Drawing;

public readonly struct Thickness(int top, int bottom, int left, int right)
{
    public int Top { get; } = top;
    public int Bottom { get; } = bottom;
    public int Left { get; } = left;
    public int Right { get; } = right;

    public static Thickness Of(int top = 0, int bottom = 0, int left = 0, int right = 0)
    {
        return new Thickness(top, bottom, left, right);
    }

    public static Thickness Of(int topBottom, int leftRight)
    {
        return new Thickness(topBottom, topBottom, leftRight, leftRight);
    }

    public static Thickness Of(int all)
    {
        return new Thickness(all, all, all, all);
    }
}