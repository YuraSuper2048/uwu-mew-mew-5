using Discord;

namespace uwu_mew_mew_5;

public static class Logger
{
    public static void Log(string message, LogSeverity logSeverity = LogSeverity.Info)
    {
        var log = $"[{DateTimeOffset.Now:MM/dd/yy HH:mm:ss}] [{logSeverity}] {message}";
        Console.WriteLine(log);
        File.AppendAllText("log.txt", log + '\n');
    }

    public static void UserLog(IUser user, string message, LogSeverity logSeverity = LogSeverity.Info)
    {
        var log = $"[{DateTimeOffset.Now:MM/dd/yy HH:mm:ss}] [{logSeverity}] [{user.Username}] {message}";
        Console.WriteLine(log);
        File.AppendAllText("log.txt", log + '\n');
    }

    public static void UserChannelLog(IUser user, IChannel channel, string message, LogSeverity logSeverity = LogSeverity.Info)
    {
        string channelString;
        if (channel is IGuildChannel guildChannel)
            channelString = $"#{channel.Name} in \"{guildChannel.Guild.Name}\"";
        else
            channelString = $"DM";
        var log = $"[{DateTimeOffset.Now:MM/dd/yy HH:mm:ss}] [{logSeverity}] [{user.Username}] [{channelString}] {message}";
        Console.WriteLine(log);
        File.AppendAllText("log.txt", log + '\n');
    }
}