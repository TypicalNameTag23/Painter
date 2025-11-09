namespace Taui.Exceptions;

/// <summary>
/// Base <see cref="Exception"/> class for all Taui exceptions
/// </summary>
public class TauiException(string message) : Exception(message);