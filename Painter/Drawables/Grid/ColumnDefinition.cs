namespace Painter.Drawables;

public record ColumnDefinition(GridLength Length)
{
    public static ColumnDefinition[] Of(string definitions)
    {
        return GridLength
            .ParseLengths(definitions)
            .Select(length => new ColumnDefinition(length))
            .ToArray();
    }
}