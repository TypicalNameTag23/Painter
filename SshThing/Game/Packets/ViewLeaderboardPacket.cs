using System.Buffers;
using TicTacToeServer.Packets;

namespace SshThing.Game.Packets;

public class ViewLeaderboardPacket : AbstractPacket
{
    public override PacketType PacketType => PacketType.ViewLeaderboard;
}