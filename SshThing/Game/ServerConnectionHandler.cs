using System.IO.Pipelines;
using System.Net.Sockets;
using SshThing.Game;
using SshThing.Game.Packets;
using SshThing.Utilities;
using SshThing.Views;
using TicTacToeServer.Packets;

namespace SshThing;

public class ServerConnectionHandler(string address, int port)
{
     private static readonly TimeSpan TimeOut = TimeSpan.FromSeconds(5);

     private CancellationTokenSource _cancellationTokenSource;
     private List<AbstractPacket> _pendingPackets = [];
     private Pipe _pipe = new();
     private TcpClient? _server;

     public EventHandler<EventArgs>? OnConnectionClosed;
     public bool Connected => _server != null;
     
     public async Task<bool> TryConnectAsync(CancellationTokenSource cancellationTokenSource)
     {
          _cancellationTokenSource = cancellationTokenSource;
          if (Connected)
          {
               return true;
          }
          
          try
          {
               _server = new TcpClient();
               await _server.ConnectAsync(address, port);
               _ = ProcessPacketsAsync();
          }
          catch (SocketException e)
          {
               Console.WriteLine("Failed to connect to the backend server, is it up?");
               Console.WriteLine(e);

               _server!.Dispose();
               _server = null;
       
               return false;
          }
          
          return true;
     }

     public async Task DisconnectAsync()
     {
          await _cancellationTokenSource.CancelAsync();
          await _server!.SendAsync(new CloseConnectionPacket());
          
          _server?.Close();
          _server?.Dispose();
          _server = null;
          
          OnConnectionClosed?.Invoke(this, EventArgs.Empty);
     }

     public async Task<bool> TrySendAsync(AbstractPacket packet)
     {
          try
          {
               await _server!.SendAsync(packet);
          }
          catch (Exception e)
          {
               return false;
          }
          
          return true;
     }

     public async Task<AbstractPacket?> GetResponseAsync(PacketType packetType, CancellationToken cancellationToken = default)
     {
          var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
          var token = linkedTokenSource.Token;
          var start = DateTime.Now;
          while (!token.IsCancellationRequested)
          {
               for (var i = 0; i < _pendingPackets.Count; i++)
               {
                    if (_pendingPackets[i].PacketType != packetType)
                    {
                         continue;
                    }
                    
                    var packet = _pendingPackets[i];
                    _pendingPackets.RemoveAt(i);
                    
                    return packet;
               }

               if (DateTime.Now - start > TimeOut)
               {
                    await DisconnectAsync();
               }
          }
          
          return null;
     }

     private async Task ProcessPacketsAsync()
     {
          var writing = FillPipeAsync();
          var reading = ReadPipeAsync();

          try
          {
               await Task.WhenAll(writing, reading);
          }
          catch (OperationCanceledException) { }
          catch (Exception e)
          {
               await DisconnectAsync();
          }
     }

     private async Task FillPipeAsync()
     {
          const int minimumBufferSize = 512;
          
          var stream = _server!.GetStream();
          var writer = _pipe.Writer;
          while (!_cancellationTokenSource.IsCancellationRequested)
          {
               var memory = writer.GetMemory(minimumBufferSize);
               try
               {
                    var bytesRead = await stream.ReadAsync(memory, _cancellationTokenSource.Token);
                    if (bytesRead == 0)
                    {
                         break;
                    }
                    
                    writer.Advance(bytesRead);
               }
               catch (Exception e)
               {
                    Console.WriteLine(e);
                    break;
               }

               var result = await writer.FlushAsync(_cancellationTokenSource.Token);
               if (result.IsCompleted)
               {
                    break;
               }
          }
          
          await writer.CompleteAsync();
     }

     private async Task ReadPipeAsync()
     {
          var reader = _pipe.Reader;
          while (!_cancellationTokenSource.IsCancellationRequested)
          {
               var result = await reader.ReadAsync(_cancellationTokenSource.Token);
               var buffer = result.Buffer;
               reader.AdvanceTo(buffer.Start, buffer.End);
               
               if (!PacketDecoder.TryReadPacket(ref buffer, out var packet))
               {
                    return;
               }
               
               await HandlePacketAsync(packet!);
               if (result.IsCompleted)
               {
                    break;
               }
          }

          await reader.CompleteAsync();
     }

     private async Task HandlePacketAsync(AbstractPacket packet)
     {
          Console.WriteLine($"Received packet: {packet}");
          switch (packet.PacketType)
          {
               // case PacketType.LobbyCreate:
                    // break;
               // case PacketType.LobbyDisconnect:
                    // break;
               // case PacketType.LobbyJoin:
                    // break;
               // case PacketType.ViewLeaderboard:
                    // break;
               case PacketType.LobbyInfo:
               case PacketType.Denied:
               case PacketType.Accepted:
               {
                    if (packet == null)
                    {
                         Console.WriteLine("WWTTFFFFFF");
                    }
                    
                    _pendingPackets.Add(packet);
                    break;
               }
               case PacketType.CloseConnection:
               {
                    OnConnectionClosed?.Invoke(this, EventArgs.Empty);
                    break;
               }
               default:
               {
                    Console.WriteLine($"Received client side packet from server, dropping.");
                    break;
               }
          }
     }
}