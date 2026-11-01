using System.Net.NetworkInformation;
using System.Net.Sockets;
using TicTacToeServer.Packets;

namespace SshThing.Utilities;

public static class TcpClients
{
    public static async Task SendAsync(this TcpClient tcpClient, byte[] message)
    {
        var stream = tcpClient.GetStream();
        await stream.WriteAsync(message);
    }

    public static async Task SendAsync(this TcpClient tcpClient, AbstractPacket packet)
    {
        await tcpClient.SendAsync(packet.Encode());
    }
}