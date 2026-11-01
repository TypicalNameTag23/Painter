using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using SshThing.Game.Packets;
using TicTacToeServer.Packets;
using TicTacToeServer.Packets.Lobby;
using TicTacToeServer.Utilities;

namespace SshThing.Game;

public static class PacketDecoder
{
    private static readonly Dictionary<PacketType, Func<AbstractPacket>> PacketRegistry = new()
    {
        // [PacketType.LobbyCreate] = () => new LobbyCreatePacket(),
        // [PacketType.LobbyDisconnect] = () => new LobbyCreatePacket() todo
        // [PacketType.LobbyJoin] = () => new LobbyJoinPacket(),
        [PacketType.LobbyInfo] = () => new LobbyInfoPacket()
        // [PacketType.LobbyJoinRejected] = () => new LobbyJoinRejectionPacket() client side
        // [PacketType.CloseConnection] = () => new CloseConnectionPacket(),
        // [PacketType.ViewLeaderboard] = () => new ViewLeaderboardPacket(),
    };

    public static bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out AbstractPacket? packet)
    {
        packet = null;
        if (buffer.Length < 2)
        {
            return false;
        }
        
        try
        {
            var slice = buffer.Slice(0, AbstractPacket.PacketTypeSizeBytes).FirstSpan;
            if (BitConverter.IsLittleEndian)
            {
                slice = slice.Reversed();
            }
            
            var packetId = BitConverter.ToInt16(slice);
            var packetType = (PacketType) packetId;
            if (!PacketRegistry.TryGetValue(packetType, out var packetFactory))
            {
                Console.WriteLine($"Failed to decode packet: unknown packet Id {packetId}");
                return false;
            }

            packet = packetFactory();
            var bytesRead = packet.Load(buffer);
            if (bytesRead == -1)
            {
                return false;
            }

            buffer = buffer.Slice(bytesRead + AbstractPacket.PacketTypeSizeBytes);
            
            return true;
        }
        catch (EndOfStreamException e)
        {
            Console.WriteLine(e);
            return false;
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Received ");
            return false;
        }
    }
}