using System.Buffers;
using SshThing.Game.Packets;

namespace TicTacToeServer.Packets.Lobby;

public class LobbyCreatePacket : IPacket
{
    public PacketType PacketType => PacketType.LobbyCreate;

    public int Load(ReadOnlySequence<byte> buffer) => 0;
}