namespace SshThing;

class Program
{
    public static async Task Main()
    {
        var server = new SshServer();
        server.Start();

        await Task.Delay(Timeout.Infinite);
    }
}