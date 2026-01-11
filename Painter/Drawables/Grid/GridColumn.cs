namespace Painter.Drawables;

public class GridColumn(ColumnDefinition columnDefinition) : ContentControl
{
    public ColumnDefinition ColumnDefinition { get; } = columnDefinition;
}