using SshThing.Game.Packets;

namespace TicTacToeServer.Packets.Lobby;

public class LobbyCreatePacket : AbstractPacket
{
    public override PacketType PacketType => PacketType.LobbyCreate;
}