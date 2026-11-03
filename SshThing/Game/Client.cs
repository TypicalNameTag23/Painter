using FxSsh;
using FxSsh.Services;
using Painter;
using Painter.Drawables;
using Sample;
using SshThing.Views;

namespace SshThing.Game;

public class Client : IDisposable
{
    private static readonly Key CtrlCKey = new(ConsoleKey.C, ConsoleModifiers.Control);

    private CancellationTokenSource _cancellationTokenSource;
    private ContentControl _contentDisplay;
    private WelcomeView _welcomeView;
    private Application _app;

    private readonly PtyArgs _args;
    private Session _session;

    public Client(PtyArgs args, Session session)
    {
        _args = args;
        _session = session;
    }

    public async Task StartAsync()
    {
        #region Setup application views
        _welcomeView = new WelcomeView
        {
            OnCreateLobbyClick = OnCreateLobbyClick,
            OnJoinLobbyClick = OnJoinLobbyClick,
            OnLeaderboardClick = OnLeaderboardClick
        };
        
        _contentDisplay = new ContentControl
        {
            Content = _welcomeView
        };
        #endregion

        #region Setup application
        var terminal = new SshTerminal(_args, new XTermInputParser());
        _app = new Application(terminal)
        {
            Content = new Grid { RowDefinitions = RowDefinition.Of("1, *") }
                .SetContent(new FpsTacker())
                .SetContent(_contentDisplay, row: 1)
        };
        _app.AddGlobalKeyBind(CtrlCKey, (_, _) => { DisconnectAsync().Wait(); });
        #endregion
        
        var applicationThread = new Thread(_app.Start);
        applicationThread.Start();
    }

    public async Task DisconnectAsync()
    {
        _app.Kill();
        _session.Disconnect();
    }

    private void OnCreateLobbyClick()
    {
        _contentDisplay.Content = new GameView();
        UpdateCtrlCKeyBind();
        Console.WriteLine("Create lobby button clicked!");
    }

    private void OnJoinLobbyClick()
    {
        _contentDisplay.Content = new JoinLobbyView(OnJoinButtonClick);
        UpdateCtrlCKeyBind();
        Console.WriteLine("Join lobby button clicked!");
    }
    
    private void OnJoinButtonClick(string code)
    {
        _contentDisplay.Content = new GameView();
        Console.WriteLine("Join button clicked!");
    }

    private void OnLeaderboardClick()
    {
        _contentDisplay.Content = new LeaderboardView();
        UpdateCtrlCKeyBind();
        Console.WriteLine("Leaderboard button clicked!");
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
            _app.AddGlobalKeyBind(CtrlCKey, (_, _) => { DisconnectAsync().Wait(); });
        });
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}