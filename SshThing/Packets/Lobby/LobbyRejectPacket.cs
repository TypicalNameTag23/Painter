using System.Buffers;
using SshThing.Game.Packets;
using TicTacToeServer.Utilities;

namespace TicTacToeServer.Packets.Lobby;

public class LobbyJoinRejectionPacket : IPacket
{
    private const int RejectionReasonSizeBytes = sizeof(short);
    
    public LobbyJoinRejectionPacket() { }

    public LobbyJoinRejectionPacket(LobbyJoinRejectionReason rejectionReason)
    {
        RejectionReason = rejectionReason;
    }

    public PacketType PacketType => PacketType.LobbyJoinRejected;
    public string Message => $"{PacketType}{RejectionReason}";
    public LobbyJoinRejectionReason RejectionReason { get; private set; }
    
    public byte[] Encode()
    {
        var packetTypeBytes = BitConverter.GetBytes((short) PacketType);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(packetTypeBytes);
        }
        
        var rejectionReasonBytes = BitConverter.GetBytes((short) RejectionReason);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(rejectionReasonBytes);
        }

        var encoded = new byte[IPacket.PacketTypeSizeBytes + RejectionReasonSizeBytes];
        Buffer.BlockCopy(
            packetTypeBytes, 0, 
            encoded, 0, IPacket.PacketTypeSizeBytes);
        Buffer.BlockCopy(
            rejectionReasonBytes, IPacket.PacketTypeSizeBytes, 
            encoded, IPacket.PacketTypeSizeBytes, RejectionReasonSizeBytes);

        return encoded;
    }

    // todo little endian validate data length
    public int Load(ReadOnlySequence<byte> buffer)
    {
        if (buffer.Length < RejectionReasonSizeBytes)
        {
            return -1;
        }

        var slice = buffer
            .Slice(IPacket.PacketTypeSizeBytes, RejectionReasonSizeBytes)
            .FirstSpan;
        
        if (BitConverter.IsLittleEndian)
        {
            slice = slice.Reversed();
        }
        
        var data = BitConverter.ToInt16(slice);
        RejectionReason = (LobbyJoinRejectionReason) data;
        
        return RejectionReasonSizeBytes;
    }
}

public enum LobbyJoinRejectionReason : short
{
    LobbyFull = 0,
    LobbyNotFound = 1,
    Unknown = 2
}