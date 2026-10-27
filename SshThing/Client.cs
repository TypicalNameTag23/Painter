using System.Net.NetworkInformation;
using System.Net.Sockets;
using FxSsh;
using FxSsh.Services;
using Painter;
using Painter.Drawables;
using Sample;
using SshThing.Packets;
using SshThing.Utilities;
using SshThing.Views;

namespace SshThing;

public class Client : IDisposable
{
    private const int BackendServerPort = 4343;
    private const string BackendServerAddress = "127.0.0.1";
    private static readonly Key CtrlCKey = new(ConsoleKey.C, ConsoleModifiers.Control);
    
    private readonly Session _session;
    private readonly Application _app;
    private readonly Thread _applicationThread;
    private readonly ContentControl _contentDisplay;
    private readonly WelcomeView _welcomeView;
    private readonly DisconnectedView _disconnectedView;
    
    private TcpClient? _server;

    public Client(PtyArgs args, Session session)
    {
        _welcomeView = new WelcomeView
        {
            OnCreateLobbyClick = OnCreateLobbyClick,
            OnJoinLobbyClick = OnJoinLobbyClick,
            OnLeaderboardClick = ContentOnLeaderboardClick
        };
        
        _contentDisplay = new ContentControl
        {
            Content = _welcomeView
        };
        
        var terminal = new SshTerminal(args, new XTermInputParser());
        _session = session;
        _app = new Application(terminal)
        {
            Content = new Grid { RowDefinitions = RowDefinition.Of("1, *") }
                .SetContent(new FpsTacker())
                .SetContent(_contentDisplay, row: 1)
        };
        _app.AddGlobalKeyBind(CtrlCKey, (_, _) => { Disconnect(); });


        _disconnectedView = new DisconnectedView
        {
            OnReconnectButtonClick = OnReconnectClick
        };
        
        var connected = ConnectToBackendServer();
        if (!connected)
        {
            _contentDisplay.Content = _disconnectedView;
        }
        
        _applicationThread = new Thread(_app.Start);
        _applicationThread.Start();
    }

    private void OnReconnectClick()
    {
        if (ConnectToBackendServer())
        {
            _contentDisplay.Content = _welcomeView;
        }
    }

    public void Disconnect()
    {
        if (ConnectedToServer())
        {
            _server!.Send(new DisconnectPacket());
        }

        _app.Kill();
        _session.Disconnect();
        _server?.Close();
    }

    private bool ConnectToBackendServer()
    {
        try
        {
            _server = new TcpClient();
            _server.Connect(BackendServerAddress, BackendServerPort);
        }
        catch (SocketException e)
        {
            Console.WriteLine("Failed to connect to the backend server, is it up?");
            Console.WriteLine(e);
            
            return false;
        }

        return true;
    }

    private void OnCreateLobbyClick()
    {
        if (!ValidateServerConnection())
        {
            return;
        }

        _server!.Send(new CreateLobbyPacket());
        _contentDisplay.Content = new GameView();
        UpdateCtrlCKeyBind();
        
        Console.WriteLine("Create lobby button clicked!");
    }

    private void OnJoinLobbyClick()
    {
        Console.WriteLine("Join lobby button clicked!");
        _contentDisplay.Content = new JoinLobbyView(code =>
        {
            if (!ValidateServerConnection())
            {
                return;
            }

            _server!.Send(new JoinLobbyPacket(code));
            _contentDisplay.Content = new GameView();
            
            Console.WriteLine("Join button clicked!");
        });

        UpdateCtrlCKeyBind();
    }

    private void ContentOnLeaderboardClick()
    {
        if (!ValidateServerConnection())
        {
            return;
        }

        _server!.Send(new ViewLeaderboardPacket());
        _contentDisplay.Content = new LeaderboardView();
        UpdateCtrlCKeyBind();
        
        Console.WriteLine("Leaderboard button clicked!");
    }

    private bool ValidateServerConnection()
    {
        var connected = ConnectedToServer();
        if (!connected)
        {
            _contentDisplay.Content = _disconnectedView;
            _server?.Close();
            _server?.Dispose();
            _server = null;
        }

        return connected;
    }

    private bool ConnectedToServer()
    {
        return _server != null && _server.GetState() == TcpState.Established;
    }

    // a bunch of stupid hacks since there is no way of interacting with the main application
    // to change the displayed content nor is there a way to add a page specific key bind
    // and there is also no concept of a back stack, so its either create a new welcome window
    // or this hack
    private void UpdateCtrlCKeyBind()
    {
        _app.RemoveGlobalBoundKey(CtrlCKey);
        _app.AddGlobalKeyBind(CtrlCKey, (_, _) =>
        {
            _contentDisplay.Content = _welcomeView;
            _app.RemoveGlobalBoundKey(CtrlCKey);
            _app.AddGlobalKeyBind(CtrlCKey, (_, _) => { Disconnect(); });
        });
    }

    public void Dispose()
    {
        _server?.Dispose();
    }
}