namespace SshThing.Packets;

public class DisconnectPacket : IPacket
{
    public string PacketType => "Disconnect";
}