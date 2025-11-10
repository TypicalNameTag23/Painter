namespace Painter.Drawing;

/// <summary>
/// Represents a pixel on the screen.
/// </summary>
/// <param name="Character">The character used by the pixel</param>
/// <param name="ForegroundColor">The foreground color of this pixel, default is null.</param>
/// <param name="BackgroundColor">The background color of this pixel, default is null.</param>
/// <seealso cref="ConsoleColor"/>
public record Pixel(char Character, ConsoleColor? ForegroundColor = null, ConsoleColor? BackgroundColor = null);