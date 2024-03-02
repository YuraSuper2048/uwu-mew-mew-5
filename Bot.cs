using Discord;
using Discord.WebSocket;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace uwu_mew_mew_5;

public static class Bot
{
    public static DiscordSocketClient Client { get; private set; } = null!;
    public static SocketSelfUser User => Client.CurrentUser;

    public static string Version = "5.3.0";

    public static async Task RunAsync()
    {
        await InitClient();

        await Task.Delay(-1);
    }

    private static async Task KeepAlive()
    {
        var reconnects = new List<DateTimeOffset>();
        while (true)
        {
            // Each 30 seconds,
            await Task.Delay(TimeSpan.FromSeconds(30));
            // If we are disconnected,
            if (Client.ConnectionState == ConnectionState.Connected) continue;

            // And we have been retrying to connect more than 100 times in the last hour,
            if (reconnects.Count(d => (DateTimeOffset.UtcNow - d).TotalMinutes < 60) > 100)
                await Task.Delay(TimeSpan.FromMinutes(15)); // Wait 15 minutes before the next check

            // If we are still disconnected after 10 seconds,
            await Task.Delay(TimeSpan.FromSeconds(10));
            if (Client.ConnectionState == ConnectionState.Connected) continue;

            // Logout and reconnect
            reconnects.Add(DateTimeOffset.UtcNow);
            await Client.LogoutAsync();
            await Client.DisposeAsync();
            var task = InitClient();
            return;
        }
    }
    
    private static async Task InitClient()
    {
        var config = new DiscordSocketConfig();
        config.GatewayIntents = GatewayIntents.AllUnprivileged 
                                | GatewayIntents.MessageContent 
                                & ~GatewayIntents.GuildInvites 
                                & ~GatewayIntents.GuildScheduledEvents;
        config.MessageCacheSize = 100;
        Client = new DiscordSocketClient(config);

        Client.Log += Log;

        Client.Ready += Client_OnReady;
        Client.MessageReceived += Client_OnMessageReceived;
        Client.ButtonExecuted += Client_OnButtonExecuted;
        Client.SelectMenuExecuted += Client_OnSelectMenuExecuted;
        Client.SlashCommandExecuted += Client_OnSlashCommandExecuted;
        Client.ModalSubmitted += Client_OnModalSubmitted;

        await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_AUTH_TOKEN"));
        await Client.StartAsync();

        await Client.SetStatusAsync(UserStatus.Online);
    }

    private static Task Log(LogMessage message)
    {
        Logger.Log(message.Message + (message.Exception == null ? "" : message.Exception), message.Severity);
        return Task.CompletedTask;
    }

    private static Task Client_OnReady()
    {
        _ = KeepAlive();
        _ = CreateSlashCommands();
        
        return Task.CompletedTask;
    }

    private static async Task Client_OnMessageReceived(SocketMessage message)
    {
        if (message.Author.IsBot) return;
        if (message is not SocketUserMessage userMessage) return;
        
        if (message.MentionedUsers.Select(x => x.Id).Contains(User.Id) && !message.Author.IsBot)
        {
            _ = Ai.OnMessageReceived(userMessage);
        }
        
        if (message.Channel is IDMChannel && !message.Author.IsBot)
        {
            _ = Ai.OnMessageReceived(userMessage);
        }
    }

    private static async Task Client_OnButtonExecuted(SocketMessageComponent component)
    {
        await Ai.OnButtonExecuted(component);
    }

    private static async Task Client_OnSelectMenuExecuted(SocketMessageComponent component)
    {
        await Ai.OnSelectMenuExecuted(component);
    }

    private static async Task Client_OnSlashCommandExecuted(SocketSlashCommand command)
    {
        await Ai.OnSlashCommandExecuted(command);
    }

    private static async Task Client_OnModalSubmitted(SocketModal modal)
    {
        await Ai.OnModalSubmitted(modal);
    }

    private static readonly Ai Ai = new();

    private static async Task CreateSlashCommands()
    {
        ApplicationCommandProperties[] commands =
        [
            new SlashCommandBuilder()
                .WithName("uwu-reset")
                .WithDescription("Resets your conversation with uwu mew mew")
                .Build(),

            new SlashCommandBuilder()
                .WithName("uwu-settings")
                .WithDescription("Opens uwu mew mew's settings menu")
                .Build(),
            
            new SlashCommandBuilder()
                .WithName("uwu-character-manager")
                .WithDescription("Opens uwu mew mew's character manager")
                .Build()
        ];

        await Client.BulkOverwriteGlobalApplicationCommandsAsync(commands);
    }
}