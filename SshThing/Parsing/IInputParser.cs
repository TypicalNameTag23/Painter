using Painter;

namespace ConsoleApp4;

/// <summary>
/// Offers a way to statefully parse data received from an SSH client
/// </summary>
public interface IInputParser
{
    public KeyInfo? Parse(byte[] input);
}