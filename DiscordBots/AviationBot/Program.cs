using Discord;
using Discord.WebSocket;

public class Program
{
    private DiscordSocketClient _client;

    public static void Main(string[] args)
    {
        new Program().MainAsync().GetAwaiter().GetResult();
        SavedData.Instance.CreateSaveData();
    }

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;

        var token = "token";




        await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
        await _client.StartAsync();

        //Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}