using System.Buffers;
using SshThing.Game.Packets;
using TicTacToeServer.Utilities;

namespace TicTacToeServer.Packets.Lobby;

public class LobbyJoinPacket : IPacket
{
    private const int LobbyIdSizeBytes = sizeof(int);
    
    public PacketType PacketType => PacketType.LobbyJoin;
    public string Message => $"{(short) PacketType}{LobbyId}";
    public int LobbyId { get; private set; }
    
    public LobbyJoinPacket() { }

    public LobbyJoinPacket(int lobbyId)
    {
        LobbyId = lobbyId;
    }

    public byte[] Encode()
    {
        var packetTypeBytes = BitConverter.GetBytes((short) PacketType);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(packetTypeBytes);
        }
        
        var lobbyIdBytes = BitConverter.GetBytes(LobbyId);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(lobbyIdBytes);
        }

        var encoded = new byte[IPacket.PacketTypeSizeBytes + LobbyIdSizeBytes];
        Buffer.BlockCopy(
            packetTypeBytes, 0, 
            encoded, 0, IPacket.PacketTypeSizeBytes);
        Buffer.BlockCopy(
            lobbyIdBytes, 0, 
            encoded, IPacket.PacketTypeSizeBytes, LobbyIdSizeBytes);

        return encoded;
    }

    // todo little endian validate data length
    public int Load(ReadOnlySequence<byte> buffer)
    {
        if (buffer.Length < LobbyIdSizeBytes)
        {
            return -1;
        }

        var slice = buffer
            .Slice(IPacket.PacketTypeSizeBytes, LobbyIdSizeBytes)
            .FirstSpan;
        
        if (BitConverter.IsLittleEndian)
        {
            slice = slice.Reversed();
        }
        
        var data = BitConverter.ToInt32(slice);
        LobbyId = data;
        
        return LobbyIdSizeBytes;
    }
}