using SimpleOpenAi.ChatEndpoint;

namespace uwu_mew_mew_5;

public struct Conversation
{
    public Conversation() : this([], "gpt-4", "uwu-mew-mew")
    {
        
    }

    public Conversation(List<Message> messages, string model, string character)
    {
        Messages = messages;
        Model = model;
        Character = character;
    }

    public List<Message> Messages;
    public string Model;
    public string Character;

    public readonly void Deconstruct(out List<Message> messages, out string model, out string character)
    {
        messages = Messages;
        model = Model;
        character = Character;
    }
}