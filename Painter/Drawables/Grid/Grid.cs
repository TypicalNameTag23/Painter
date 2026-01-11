using System.Drawing;
using Painter.Drawing.Canvas;

// ReSharper disable once CheckNamespace
namespace Painter.Drawables;

public class Grid : Drawable
{
    private RowDefinition[] _rowDefinitions;
    private ColumnDefinition[] _columnDefinitions;
    private ContentControl[,] _content;
    private int _previousWidth;
    private int _previousHeight;

    public Grid()
    { 
        _rowDefinitions = RowDefinition.Of("*");
        _columnDefinitions = ColumnDefinition.Of("*");
        _content = new ContentControl[1, 1];

        var contentControl = new ContentControl();
        Attach(contentControl);
        _content[0, 0] = contentControl;
    }
    
    public IReadOnlyList<RowDefinition> RowDefinitions
    {
        get => _rowDefinitions;
        set
        {
            if (_rowDefinitions == value)
            {
                return;
            }

            _rowDefinitions = value.ToArray();
            UpdateContentArray();
            Measure();
            RequestRedraw();
        }
    }
    
    public IReadOnlyList<ColumnDefinition> ColumnDefinitions
    {
        get => _columnDefinitions;
        set
        {
            if (_columnDefinitions == value)
            {
                return;
            }

            _columnDefinitions = value.ToArray();
            UpdateContentArray();
            Measure();
            RequestRedraw();
        }
    }

    public Grid SetContent(int row, int column, Drawable content)
    {
        if (row >= _rowDefinitions.Length || row < 0 || column >= _columnDefinitions.Length || column < 0)
        {
            return this;
        }

        _content[row, column].Content = content;
        return this;
    }

    public override void Draw(ICanvas canvas)
    {
        if (_previousHeight != Height || _previousWidth != Width)
        {
            Measure();
            _previousHeight = Height;
            _previousWidth = Width;
        }
        
        base.Draw(canvas);
    }

    /// <summary>
    /// Measures the grid
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    private void Measure()
    {
        if (Width == 0 && Height == 0)
        {
            return;
        }
        
        var availableWidth = MeasureAbsoluteColumns();
        var finalStarWidth = Width;
        var starColumns = GetStarColumns();
        var columnPercentage = 1 / starColumns.Sum(column => column.Item2.Length.Value);
        foreach (var (index, column) in starColumns)
        {
            var width = index == _columnDefinitions.Length - 1 
                ? finalStarWidth 
                : (int) (availableWidth * column.Length.Value * columnPercentage);
            
            finalStarWidth -= width;
            for (var row = 0; row < _columnDefinitions.Length; row++)
            {
                _content[row, index].Width = width;
            }
        }

        var availableHeight = MeasureAbsoluteRows();
        var finalStarHeight = Height;
        var starRows = GetStarRows();
        var rowPercentage = 1 / starRows.Sum(row => row.Item2.Length.Value);
        foreach (var (index, row) in starRows)
        {
            var height = index == _rowDefinitions.Length - 1
                ? finalStarHeight
                : (int)(availableHeight * row.Length.Value * rowPercentage);
            
            finalStarHeight -= height;
            for (var column = 0; column < _columnDefinitions.Length; column++)
            {
                _content[index, column].Height = height;
            }
        }

        var y = 0;
        for (var row = 0; row < _rowDefinitions.Length; row++)
        {
            var x = 0;
            for (var column = 0; column < _columnDefinitions.Length; column++)
            {
                var contentControl = _content[row, column];
                contentControl.X = x;
                contentControl.Y = y;
                x += contentControl.Width;

                if (column == _columnDefinitions.Length - 1)
                {
                    y += contentControl.Height;
                }
            }
        }
    }

    private int MeasureAbsoluteColumns()
    {
        var availableWidth = Width;
        foreach (var (columnIndex, absoluteColumn) in GetAbsoluteColumns())
        {
            var width = (int) absoluteColumn.Length.Value;
            if (availableWidth - width < 0)
            {
                width = availableWidth;
                availableWidth = 0;
            }
            else
            {
                availableWidth -= width;
            }
            
            for (var row = 0; row < _columnDefinitions.Length; row++)
            {
                _content[row, columnIndex].Width = width;
            }
        }

        return availableWidth;
    }

    private int MeasureAbsoluteRows()
    {
        var availableHeight = Height;
        foreach (var (rowIndex, absoluteRow) in GetAbsoluteRows())
        {
            var height = (int) absoluteRow.Length.Value;
            if (availableHeight - height < 0)
            {
                height = availableHeight;
                availableHeight = 0;
            }
            else
            {
                availableHeight -= height;
            }
            
            for (var column = 0; column < _columnDefinitions.Length; column++)
            {
                _content[rowIndex, column].Height = height;
            }
        }

        return availableHeight;
    }

    private List<(int, ColumnDefinition)> GetAbsoluteColumns()
    {
        var columns = new List<(int, ColumnDefinition)>();
        for (var column = 0; column < _columnDefinitions.Length; column++)
        {
            if (_columnDefinitions[column].Length.IsAbsolute)
            {
                columns.Add((column, _columnDefinitions[column]));
            }
        }

        return columns;
    }

    private List<(int, RowDefinition)> GetAbsoluteRows()
    {
        var rows = new List<(int, RowDefinition)>();
        for (var row = 0; row < _rowDefinitions.Length; row++)
        {
            if (_rowDefinitions[row].Length.IsAbsolute)
            {
                rows.Add((row, _rowDefinitions[row]));
            }
        }

        return rows;
    }

    private List<(int, ColumnDefinition)> GetStarColumns()
    {
        var columns = new List<(int, ColumnDefinition)>();
        for (var column = 0; column < _columnDefinitions.Length; column++)
        {
            if (_columnDefinitions[column].Length.IsStar)
            {
                columns.Add((column, _columnDefinitions[column]));
            }
        }

        return columns;
    }

    private List<(int, RowDefinition)> GetStarRows()
    {
        var rows = new List<(int, RowDefinition)>();
        for (var row = 0; row < _rowDefinitions.Length; row++)
        {
            if (_rowDefinitions[row].Length.IsStar)
            {
                rows.Add((row, _rowDefinitions[row]));
            }
        }

        return rows;
    }
    
    private void UpdateContentArray()
    {
        foreach (var contentControl in _content)
        {
            Detach(contentControl);
        }
        
        _content = new ContentControl[_rowDefinitions.Length, _columnDefinitions.Length];
        for (var row = 0; row < _content.GetLength(0); row++)
        {
            for (var column = 0; column < _content.GetLength(1); column++)
            {
                var contentControl = new ContentControl();
                Attach(contentControl);
                _content[row, column] = contentControl;
            }
        }
        // var content = _content;
        // _content = new ContentControl[_rowDefinitions.Length, _columnDefinitions.Length];
        // if (content is null)
        // {
        //     return;
        // }
        //
        // if (content.GetLength(0) < _content.GetLength(0))
        // {
        //     SizeUp(content, true);
        // }
        //
        // for (var row = 0; row < content.GetLength(0); row++)
        // {
        //     for (var column = 0; column < content.GetLength(1); column++)
        //     {
        //         if (row < _rowDefinitions.Length && column < _columnDefinitions.Length)
        //         {
        //             _content[row, column] = content[row, column];
        //         }
        //         else
        //         {
        //             Detach(content[row, column]);
        //         }
        //     }
        // }
    }

    // private void SizeUp(ContentControl[,] oldContent, bool sizeUpRows)
    // {
    //     if (sizeUpRows)
    //     {
    //         for (var row = oldContent.GetLength(0) + 1; row < )
    //     }
    //     else
    //     {
    //         
    //     }
    // }
}