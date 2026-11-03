namespace Painter.Drawables;

public record RowDefinition(GridLength Length)
{
    public static RowDefinition[] Of(string definitions)
    {
        return GridLength
            .ParseLengths(definitions)
            .Select(length => new RowDefinition(length))
            .ToArray();
    }
}