using Newtonsoft.Json;
using SimpleOpenAi.ChatEndpoint;

namespace uwu_mew_mew_5;

public static class UserDatabase
{
    static UserDatabase()
    {
        Directory.CreateDirectory("user_data");
    }
    
    private static readonly Dictionary<ulong, object> Locks = new();

    private static readonly Dictionary<ulong, UserData> Cache = new();
    
    public static async Task PutAsync(ulong user, UserData userData)
    {
        Locks.TryAdd(user, new object());

        lock (Locks[user])
        {
            var path = Path.Combine("user_data", $"{user}.json");
            var data = JsonConvert.SerializeObject(userData);
            
            File.WriteAllText(path, data);

            Cache.Remove(user);
            Cache.Add(user, userData);
        }
    }
    
    public static async Task<UserData> GetAsync(ulong user)
    {
        if (Cache.TryGetValue(user, out var userData))
        {
            return userData;
        }
        
        Locks.TryAdd(user, new object());

        lock (Locks[user])
        {
            var path = Path.Combine("user_data", $"{user}.json");

            if (!File.Exists(path)) return new UserData();

            var data = File.ReadAllText(path);

            userData = JsonConvert.DeserializeObject<UserData>(data);
            Cache.Add(user, userData);
            return userData;
        }
    }
}