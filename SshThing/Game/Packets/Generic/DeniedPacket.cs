namespace TicTacToeServer.Packets.Generic;

public class DeniedPacket : AbstractPacket
{
    public override PacketType PacketType => PacketType.Denied;
}