using System.Buffers;
using TicTacToeServer.Packets;

namespace SshThing.Game.Packets;

public class CloseConnectionPacket : AbstractPacket
{
    public override PacketType PacketType => PacketType.CloseConnection;
}