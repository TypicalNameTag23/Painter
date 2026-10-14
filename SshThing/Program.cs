namespace SshThing;

class Program
{
    public static async Task Main()
    {
        var server = new GameServer();
        server.Start();

        await Task.Delay(Timeout.Infinite);
    }
}