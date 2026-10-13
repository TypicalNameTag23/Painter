using System.Net;
using ConsoleApp4;
using FxSsh;
using FxSsh.Services;
using Painter;
using Painter.Drawables;
using Painter.Drawing;
using Painter.Drawing.Brushes;
using Sample;
using SshThing.Views;

namespace SshThing;

class Program
{
    private static Thread _thread2;
    private static Thread _thread;

    static async Task Main(string[] args)
    {
        // already public on FxSSH repo, no sensitive info transmitted so far
        var rsa2048BitPem = @"-----BEGIN PRIVATE KEY-----
            MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQClBPKsmiTxCoez
            E4Wt4nvNDVjLtkGQPUS/SbhJCL8J7pKrvsKRyXKM9GjGdxA7GKZR7sjqCWCU12oD
            +EJuf/mpcA0JsBY4gUU8bp95U5mEM+ZOr2aNXYXAXy/J6GQRyd1jgkD2pm1qsWZJ
            ahvXNJGMgx1lqI9woKU+PRssMtv1YupAxsSlo+xcfvC57fkaKIwUeCr6CkUlpIJN
            zP5lvatKcpmjiWLSRHUpypx2wgZPz4SnB2qE77UH/yu1DlcsQgbVa7nr+qMWfxP7
            Cpa+eTJC19c1GP+XxFaeqXDtuGaRlfBX/iihEcBnQuvsZJLg5jN0WKqvnpDfHgh/
            M4IopC2pAgMBAAECggEBAImt6ibV6NJvHa7sL9FXMEFxzE8SffsxEyWiBS5yLKnF
            sfu3CbEG6RrvZGeJuTIFK+caGekh78HfRGWRgSOehJe4lDgsAS4dtL1p8oYQmPnz
            L0khEKgLimdpQ37q9GrfCGZYq4jebFXjMts3u4i/JFyenC1QCHVIovWdmAk1Wc2N
            5bY+qlQZ4dSBl15cqNg0w4bbEF+yT+fOMm/raZANTr/IDIt88LcKpyimvUSGlZEE
            OWx4hXAPbF4h/tqeH4O+Xzmtq/1pWwdbwNqWwOXow320c97U4ofCuDXcy0TeOwiX
            gcPnaG99jX4Cy+IwdcVnDsJpN4FC2/sm1kGeOTRpIl0CgYEAy8gPV5RESpOyQ00j
            dr36JQoymLwXDS114tuMPF7dX5YX3S+yyhl0ADa11tVH15CzOaVSF1wfxDeb1TRs
            XcJoZBsxlH24BMPETB1ADy43pslRrkex54hcM4jDe3OYBTsVmV5A2sdxFGEKAbPn
            uWsk8jeZ9AsEV3M2vinzJmS5XVsCgYEAz04dSW0GXiTBESYmno9DJiyx3dT4T0eq
            bwtpZPCShEjU4BChwFg9V5fAmzw1iCrdYD68mwcxQYurp3Vgqo6u1YogRpeNfljq
            VpKnVDbd3a1CTYYyWw81f4HzflpmWLgq1BGKkdwD83xZaFh7Y46cm+xEtrJpiVFM
            GTagAokFvEsCgYB1EouV4g1V1wJ73c45Aq26J+CnlK+dl3d5jG5FpK6DosQ1A5kw
            uGzHTqcrND7g3jXJMWw3FWr+nH//fe2f8/drQ6A5UfytaBbXL5rE3eWFAXXWrUPM
            468swC6mNuOoZahkAx05U4lojtNj5QqEoMSKD114MfgdkYhquckCTq2brwKBgQC5
            s1zS0II6xSvZw9YmhWj+gl0WvVduFWGcNZnE3SgyrddbnCp5VdIlbAASTx4ZC2Th
            eXGUYh4CfC5ZRPFB96ywBxqggdQzEU1iHd8ctkWK9VCGh6cGIRqoTO2lCy/RW7Cp
            5ci+nls/uu2QZmqppS+vETgAfNPDOXs0vtUZUEs9/wKBgDNQonVvTTQIRbaRbxXu
            eVqxAVYBb8PSPBjfigb4/sGzu4iYaxuCHOkA8AK9B9SmGjaQHJ4h9t+kJKe9xNie
            v7sG5pguzUyd+AJIafbeh2Iryva/Nw3Shb7Jl6EX/lX3o/B9hRziWKV0IvwCUF/1
            iyxhUEyZT7ugi8eNl5zVJgmN
            -----END PRIVATE KEY-----";
        var ecdsap256Pem = @"-----BEGIN PRIVATE KEY-----
            MIGHAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBG0wawIBAQQgPHkoVMg7fVw+20dJ
            iZrIo86ikidAv9V/ImB7q8QJ3f6hRANCAAShaGB2y+jNBGKsI+r4l+Bq82q+UVUn
            lPBvdBz9mGA32F9oosJ1s6mPmsasSI5FdG0B8sbQMLD7j5/8Lcjb1P4I
            -----END PRIVATE KEY-----";
        var ecdsap384Pem = @"-----BEGIN PRIVATE KEY-----
            MIG2AgEAMBAGByqGSM49AgEGBSuBBAAiBIGeMIGbAgEBBDBgquj404eo5PGToagO
            tXFnhf8/kryAfQFlNEqrruGrGrAmQWuHjDeG2yxnMYpgYrShZANiAASvXyCM6j2f
            n5ytuT/ekSiWcd1KoGexHWxXE7AVWfdXY0o2iJpZxRIZbqhiLfIruAxYFlvNnaGh
            UNEj76uLE5jR1xdH471mzsEPWrxi/CeTm0OyQ6yQg3pQH5FFVyCR1so=
            -----END PRIVATE KEY-----";
        var ecdsap521Pem = @"-----BEGIN PRIVATE KEY-----
            MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIAGXO87cgkpAPLIWoc
            kZirXguaO7WeAFtO+z5TtfHyTLEgSUWlGhP1PZ3ZbyLf0ht6t4X46TQQn7Eyqkuy
            XgXZ0RihgYkDgYYABAAa9hQJavg/gAqUEIVoL1TucLMu1gCElMvX68BrJQoYdoNe
            gbR4mS/oiOdvU5zm4H2ABo6gDYo2Pl4W80lqL3nGdgAwdNN7udRi/A5wc39KvZ5w
            bbDmx/ly7kvagszIWafjG8Hzg5v5kKBbdYw9A+9pN2cbhWXug41xR1rLDOI6hFSn
            TA==
            -----END PRIVATE KEY-----";

        var server = new SshServer(new StartingInfo(IPAddress.Any, 22, "SSH-2.0-tic-tac-toe"));
        server.AddHostKey("rea-sha2-256", rsa2048BitPem);
        server.AddHostKey("rsa-sha2-512", rsa2048BitPem);
        server.AddHostKey("ecdsa-sha2-nistp256", ecdsap256Pem);
        server.AddHostKey("ecdsa-sha2-nistp384", ecdsap384Pem);
        server.AddHostKey("ecdsa-sha2-nistp521", ecdsap521Pem);
        
        server.ConnectionAccepted += OnConnectionAccepted;

        Console.WriteLine("SSH server listening on port 22...");
        server.Start();

        await Task.Delay(Timeout.Infinite);
    }

    private static void OnConnectionAccepted(
        object? sender,
        Session session)
    {
        Console.WriteLine("Client connected");

        session.ServiceRegistered += e_ServiceRegistered;
    }
    
    static void service_UserAuth(object sender, UserAuthArgs e)
    {
        Console.WriteLine($"Client {e.KeyAlgorithm} fingerprint: {e.Fingerprint}.");

        e.Result = true;
    }
    
    static void e_ServiceRegistered(object sender, SshService e)
    {
        var session = (Session)sender;
        Console.WriteLine($"Session {BitConverter.ToString(session.SessionId).Replace("-", "")} requesting {e.GetType().Name}.");

        switch (e)
        {
            case UserAuthService userAuthService:
            {
                userAuthService.EnableNoneAuth = true;
                userAuthService.UserAuth += service_UserAuth;
                break;
            }
            case ConnectionService connectionService:
            {
                connectionService.PtyReceived += (o, args) =>
                {
                    Console.WriteLine("Pty Received");

                    var term = new SshTerminal(args, new XTermInputParser());
                    var app = new Application(term);
                    var ctHodler = new ContentControl();
                    var ct = new Grid
                    {
                        RowDefinitions = RowDefinition.Of("1, *")
                    }.SetContent(new FpsTacker())
                    .SetContent(ctHodler, row: 1);
                    
                    var content = new WelcomeView();
                    content.OnCreateLobbyClick = () =>
                    {
                        Console.WriteLine("Create lobby button clicked!");
                        ctHodler.Content = new GameView();
                        
                        app.RemoveGlobalBoundKey(new Key(ConsoleKey.C,  ConsoleModifiers.Control));
                        app.AddGlobalKeyBind(new Key(ConsoleKey.C,  ConsoleModifiers.Control), (_, _) =>
                        {
                            ctHodler.Content = content;
                            app.RemoveGlobalBoundKey(new Key(ConsoleKey.C,  ConsoleModifiers.Control));
                            app.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), (_, _) =>
                            {
                                app.Kill();
                                session.Disconnect();
                            });
                        });
                    };
                    content.OnJoinLobbyClick = () =>
                    {
                        Console.WriteLine("Join lobby button clicked!");
                        ctHodler.Content = new JoinLobbyView(() =>
                        {
                            Console.WriteLine("Join button clicked!");
                            ctHodler.Content = new GameView();
                        });
                        
                        // a bunch of stupid hacks since there is no way of interacting with the main application
                        // to change the displayed content nor is there a way to add a page specific key bind
                        // and there is also no concept of a back stack, so its either create a new welcome window
                        // or this hack
                        app.RemoveGlobalBoundKey(new Key(ConsoleKey.C,  ConsoleModifiers.Control));
                        app.AddGlobalKeyBind(new Key(ConsoleKey.C,  ConsoleModifiers.Control), (_, _) =>
                        {
                            ctHodler.Content = content;
                            app.RemoveGlobalBoundKey(new Key(ConsoleKey.C,  ConsoleModifiers.Control));
                            app.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), (_, _) =>
                            {
                                app.Kill();
                                session.Disconnect();
                            });
                        });
                    };
                    content.OnLeaderboardClick = () =>
                    {
                        Console.WriteLine("Leaderboard button clicked!");
                        ctHodler.Content = new LeaderboardView();
                        
                        app.RemoveGlobalBoundKey(new Key(ConsoleKey.C,  ConsoleModifiers.Control));
                        app.AddGlobalKeyBind(new Key(ConsoleKey.C,  ConsoleModifiers.Control), (_, _) =>
                        {
                            ctHodler.Content = content;
                            app.RemoveGlobalBoundKey(new Key(ConsoleKey.C,  ConsoleModifiers.Control));
                            app.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), (_, _) =>
                            {
                                app.Kill();
                                session.Disconnect();
                            });
                        });
                    };

                    ctHodler.Content = content;
                    app.Content = ct;
                    app.AddGlobalKeyBind(new Key(ConsoleKey.C, ConsoleModifiers.Control), (_, _) =>
                    {
                        app.Kill();
                        session.Disconnect();
                    });
                    _thread = new Thread(app.Start);
                    _thread.Start();

                    // hack to keep FxSsh (?) from disconnecting an inactive client
                    session.ConfigureKeepalive(new TimeSpan(0));
                    // _thread2 = new Thread(() =>
                    // {
                    //     while ()
                    //     {
                    //         session.SendGlobalKeepalive();
                    //         // Console.WriteLine("a");
                    //         Thread.Sleep(5000);
                    //     }
                    // });
                    // _thread2.Start();
                };
                
                connectionService.CommandOpened += (o, args) =>
                {
                    args.Agreed = true;
                    Console.WriteLine("Command Opened");
                };
                
                break;
            }
        }
    }
}