namespace Painter.Drawing.Canvas;

public interface ICanvas
{
    /// <summary>
    /// The width of this <see cref="ICanvas"/>
    /// </summary>
    public int Width { get; protected set; }
    
    /// <summary>
    /// The height of this <see cref="ICanvas"/>
    /// </summary>
    public int Height { get; protected set; }
    
    /// <summary>
    /// Draws a character to this canvas using the provided brush at the location
    /// </summary>
    /// <param name="x">The x coord to draw at</param>
    /// <param name="y">The y coord to draw at</param>
    /// <param name="character">The character to draw</param>
    /// <param name="brush">The brush to use when drawing</param>
    /// <remarks>If either of the <see cref="x"/> or the <see cref="y"/> coords are larger than the <see cref="Width"/> and the <see cref="Height"/> nothing will be drawn</remarks>
    public void Draw(int x, int y, char character, Brush brush);
    
    /// <summary>
    /// Draws the provided brush at the location 
    /// </summary>
    /// <param name="x">The x coord to draw at</param>
    /// <param name="y">The y coord to draw at</param>
    /// <param name="brush">The brush to use when drawing</param>
    /// <remarks>If either of the <see cref="x"/> or the <see cref="y"/> coords are larger than the <see cref="Width"/> and the <see cref="Height"/> nothing will be drawn</remarks>
    public void Draw(int x, int y, Brush brush);

    /// <summary>
    /// Draws the string at the location 
    /// </summary>
    /// <param name="x">The x coord to draw at</param>
    /// <param name="y">The y coord to draw at</param>
    /// <param name="content">The object to get the string value of</param>
    /// <param name="brush">The brush to use when drawing</param>
    /// <remarks>
    /// <para>
    /// If either of the <see cref="x"/> or the <see cref="y"/> coords are larger than the <see cref="Width"/> and the <see cref="Height"/> nothing will be drawn
    /// </para>
    /// <para>
    /// Does not draw <see cref="Environment.NewLine"/> characters
    /// </para>
    /// </remarks>
    public void Draw(int x, int y, string content, Brush brush);
    
    /// <summary>
    /// Draws the return value of <see cref="object.ToString()"/> at the location 
    /// </summary>
    /// <param name="x">The x coord to draw at</param>
    /// <param name="y">The y coord to draw at</param>
    /// <param name="obj">The object to get the string value of</param>
    /// <param name="brush">The brush to use when drawing</param>
    /// <remarks>
    /// <para>
    /// If either of the <see cref="x"/> or the <see cref="y"/> coords are larger than the <see cref="Width"/> and the <see cref="Height"/> nothing will be drawn
    /// </para>
    /// <para>
    /// Does not draw <see cref="Environment.NewLine"/> characters
    /// </para>
    /// </remarks>
    public void Draw(int x, int y, object obj, Brush brush);
    
    /// <summary>
    /// Gets an <see cref="ICanvas"/> that represents a subsection of this <see cref="ICanvas"/>
    /// </summary>
    /// <param name="x">The x coordinate that this subsection should start at</param>
    /// <param name="y">The y coordinate that this subsection should start at</param>
    /// <param name="width">The width of this subsection</param>
    /// <param name="height">The height of this subsection</param>
    /// <remarks>
    /// If the width or the height of this subsection are lesser than 0 or greater than the <see cref="Width"/> or the <see cref="Height"/> of the parent <see cref="ICanvas"/>
    /// the <see cref="Width"/> or <see cref="Height"/> or the parent subsection will be used
    /// </remarks>
    public ICanvas Subsection(int x, int y, int width, int height);
}