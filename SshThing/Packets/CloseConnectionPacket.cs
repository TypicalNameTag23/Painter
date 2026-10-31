using System.Buffers;
using TicTacToeServer.Packets;

namespace SshThing.Game.Packets;

public class CloseConnectionPacket : IPacket
{
    public PacketType PacketType => PacketType.CloseConnection;

    public int Load(ReadOnlySequence<byte> buffer) => 0;
}