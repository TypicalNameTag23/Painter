using System.Buffers;
using TicTacToeServer.Packets;

namespace SshThing.Game.Packets;

public class ViewLeaderboardPacket(ulong id) : AbstractPacket(id)
{
    public override PacketType PacketType => PacketType.ViewLeaderboard;
}