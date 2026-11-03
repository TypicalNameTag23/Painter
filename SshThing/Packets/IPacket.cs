using System.Buffers;
using System.Text;
using SshThing.Game.Packets;

namespace TicTacToeServer.Packets;

public interface IPacket
{
    public const int PacketTypeSizeBytes = sizeof(short);
    
    /// <summary>
    /// The <see cref="PacketType"/>
    /// </summary>
    public PacketType PacketType { get; }

    /// <summary>
    /// The message sent by the packet
    /// </summary>
    public string Message => ((short) PacketType).ToString();
    
    /// <summary>
    /// Encodes the <see cref="Message"/> into a byte array
    /// </summary>
    /// <returns>The byte array</returns>
    public byte[] Encode()
    {
        var encoded = BitConverter.GetBytes((short) PacketType);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(encoded);
        }
        
        return encoded;
    }

    /// <summary>
    /// Reads in data from the buffer and populates the packet
    /// </summary>
    /// <param name="buffer">The buffer to read from</param>
    /// <returns>The number of bytes read from the buffer, -1 if nothing was loaded</returns>
    public int Load(ReadOnlySequence<byte> buffer);
}