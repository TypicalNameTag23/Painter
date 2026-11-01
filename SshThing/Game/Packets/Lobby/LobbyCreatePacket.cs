using SshThing.Game.Packets;

namespace TicTacToeServer.Packets.Lobby;

public class LobbyCreatePacket(ulong packetId) : AbstractPacket(packetId)
{
    public override PacketType PacketType => PacketType.LobbyCreate;
}