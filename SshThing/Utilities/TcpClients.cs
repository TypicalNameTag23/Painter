using System.Net.NetworkInformation;
using System.Net.Sockets;
using SshThing.Packets;

namespace SshThing.Utilities;

public static class TcpClients
{
    public static TcpState GetState(this TcpClient tcpClient)
    {
        var connInfo = IPGlobalProperties.GetIPGlobalProperties()
            .GetActiveTcpConnections()
            .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
        
        return connInfo?.State ?? TcpState.Unknown;
    }

    public static void Send(this TcpClient tcpClient, byte[] message)
    {
        var stream = tcpClient.GetStream();
        stream.Write(message);
    }

    public static void Send(this TcpClient tcpClient, IPacket packet)
    {
        tcpClient.Send(packet.Encode());
    }
}