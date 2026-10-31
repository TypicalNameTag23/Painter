namespace SshThing.Game.Packets;

public enum PacketType : short
{
    LobbyCreate = 0,
    LobbyDisconnect = 1,
    LobbyJoin = 2,
    LobbyInfo = 3,
    LobbyJoinRejected = 4,
    // LobbyJoinAccepted = 5,
    CloseConnection = 20,
    ViewLeaderboard = 40
}