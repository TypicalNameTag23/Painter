using System.Buffers;
using System.Text;
using SshThing.Game.Packets;

namespace TicTacToeServer.Packets;

public abstract class AbstractPacket
{
    public const int PacketTypeSizeBytes = sizeof(short);
    
    /// <summary>
    /// The <see cref="PacketType"/>
    /// </summary>
    public abstract PacketType PacketType { get; }

    /// <summary>
    /// The message sent by the packet
    /// </summary>
    public virtual string Message => ((short) PacketType).ToString();
    
    /// <summary>
    /// Encodes the <see cref="Message"/> into a byte array
    /// </summary>
    /// <returns>The byte array</returns>
    public virtual byte[] Encode()
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
    public virtual int Load(ReadOnlySequence<byte> buffer) => 0;
    
    public override string ToString() => $"{{ PacketType: {PacketType} }}";
}