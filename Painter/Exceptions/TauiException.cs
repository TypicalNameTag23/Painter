namespace Taui.Exceptions;

/// <summary>
/// Base <see cref="Exception"/> class for all Painter exceptions
/// </summary>
public class TauiException(string message) : Exception(message);