using System.Buffers;
using SshThing.Game.Packets;
using TicTacToeServer.Utilities;

namespace TicTacToeServer.Packets.Lobby;

public class LobbyJoinPacket : AbstractPacket
{
    private const int LobbyIdSizeBytes = sizeof(int);
    
    public override PacketType PacketType => PacketType.LobbyJoin;
    public override string Message => $"{(short) PacketType}{LobbyId}";
    public int LobbyId { get; private set; }
    
    public LobbyJoinPacket() { }

    public LobbyJoinPacket(int lobbyId)
    {
        LobbyId = lobbyId;
    }

    public override byte[] Encode()
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

        var encoded = new byte[AbstractPacket.PacketTypeSizeBytes + LobbyIdSizeBytes];
        Buffer.BlockCopy(
            packetTypeBytes, 0, 
            encoded, 0, AbstractPacket.PacketTypeSizeBytes);
        Buffer.BlockCopy(
            lobbyIdBytes, 0, 
            encoded, AbstractPacket.PacketTypeSizeBytes, LobbyIdSizeBytes);

        return encoded;
    }

    // todo little endian validate data length
    public override int Load(ReadOnlySequence<byte> buffer)
    {
        if (buffer.Length < LobbyIdSizeBytes)
        {
            return -1;
        }

        var slice = buffer
            .Slice(AbstractPacket.PacketTypeSizeBytes, LobbyIdSizeBytes)
            .FirstSpan;
        
        if (BitConverter.IsLittleEndian)
        {
            slice = slice.Reversed();
        }
        
        var data = BitConverter.ToInt32(slice);
        LobbyId = data;
        
        return LobbyIdSizeBytes;
    }
    
    public override string ToString() => $"{{ PacketType: {PacketType}, LobbyId: {LobbyId} }}";
}