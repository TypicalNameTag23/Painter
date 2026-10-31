using System.Buffers;
using TicTacToeServer.Packets;

namespace SshThing.Game.Packets;

public class ViewLeaderboardPacket : IPacket
{
    public PacketType PacketType => PacketType.ViewLeaderboard;

    public int Load(ReadOnlySequence<byte> buffer) => 0;
}