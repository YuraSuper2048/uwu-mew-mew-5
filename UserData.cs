using Newtonsoft.Json;

namespace uwu_mew_mew_5;

public struct UserData(Conversation currentConversation)
{
    public Conversation CurrentConversation = currentConversation;
    public bool FixMarkdown = false;
    public string UserDescription = "";
    public string WordRange = "150-300";
    public List<Character> Characters = [];
    public string Scenario = "";

    [JsonConstructor]
    public UserData() : this(new Conversation())
    {
        
    }
}