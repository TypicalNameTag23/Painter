using System.Text;

namespace SshThing.Packets;

public class JoinLobbyPacket(string lobbyCode) : IPacket
{
    public string PacketType => "JoinLobby";
    public string Message => $"{PacketType}\n{lobbyCode}";
}