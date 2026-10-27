namespace SshThing;

class Program
{
    public static async Task Main()
    {
        var server = new Server();
        server.Start();

        await Task.Delay(Timeout.Infinite);
    }
}