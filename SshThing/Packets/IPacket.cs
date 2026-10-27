using System.Text;

namespace SshThing.Packets;

public interface IPacket
{
    public string PacketType { get; }

    public string Message => PacketType;
    
    public byte[] Encode()
    {
        return Encoding.ASCII.GetBytes(Message);
    }
}