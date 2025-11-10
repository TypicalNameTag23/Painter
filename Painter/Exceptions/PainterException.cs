namespace Painter.Exceptions;

/// <summary>
/// Base <see cref="Exception"/> class for all Painter exceptions
/// </summary>
public class PainterException(string message) : Exception(message);