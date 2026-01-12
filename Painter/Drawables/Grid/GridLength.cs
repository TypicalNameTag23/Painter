namespace Painter.Drawables;

public record GridLength
{
    private GridLength(double value, GridLengthType type)
    {
        Value = value;
        Type = type;
    }

    /// <summary>
    /// The <see cref="GridLengthType"/>
    /// </summary>
    public GridLengthType Type { get; }

    /// <summary>
    /// The length of this grid if it is <see cref="GridLengthType.Absolute"/>
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// If the <see cref="Type"/> is <see cref="GridLengthType.Auto"/>
    /// </summary>
    // public bool IsAuto => Type == GridLengthType.Auto;

    /// <summary>
    /// If the <see cref="Type"/> is <see cref="GridLengthType.Star"/>
    /// </summary>
    public bool IsStar => Type == GridLengthType.Star;
    
    /// <summary>
    /// If the <see cref="Type"/> is <see cref="GridLengthType.Absolute"/>
    /// </summary>
    public bool IsAbsolute => Type == GridLengthType.Absolute;

    /// <summary>
    /// Gets a <see cref="GridLength"/> with <see cref="Type"/> set to <see cref="GridLengthType.Auto"/>
    /// </summary>
    // public static GridLength Auto = new(-1, GridLengthType.Auto);

    /// <summary>
    /// Creates a <see cref="GridLength"/> with <see cref="Type"/> set to <see cref="GridLengthType.Star"/>
    /// </summary>
    /// <param name="percentage">The percentage of the grid</param>
    /// <returns>A <see cref="GridLength"/></returns>
    public static GridLength Star(double percentage)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(percentage, 0,$"Percentage should be greater than 0, found {percentage} instead.");
        return new GridLength(percentage, GridLengthType.Star);
    }

    /// <summary>
    /// Creates a new <see cref="GridLength"/> with <see cref="Type"/> set to <see cref="GridLengthType.Absolute"/>
    /// </summary>
    /// <param name="length">The length</param>
    /// <returns>A <see cref="GridLength"/></returns>
    public static GridLength Absolute(int length)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(length, 0,$"Length should be greater than 0, found {length} instead.");
        return new GridLength(length, GridLengthType.Absolute);
    }

    public static GridLength Parse(string s)
    {
        if (s.EndsWith('*'))
        {
            return Star(s.Length == 1 ? 
                1 : 
                double.Parse(s[..^1]));
        }
        
        if (s.Equals("AUTO", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new NotImplementedException();
            // return s == "AUTO" ? 
            // Auto : 
            // Absolute(int.Parse(s));
                // 
        }

        return Absolute(int.Parse(s));
    }
    
    public static IEnumerable<GridLength> ParseLengths(string s)
    {
        return s
            .Split(',')
            .Select(s => s.Trim())
            .Select(Parse);
    }
}