using OpenAI;
using OpenAI.Chat;

namespace TextToSqlDemo.Services;

public class SqlGenerator
{
    private readonly ChatClient _client;

    public SqlGenerator(string apiKey)
    {
        _client = new ChatClient(model: "gpt-4o-mini", apiKey: apiKey);
    }
    
    public async Task<string> AskOPenAiChatDb(List<ChatMessage> messages)
    {
        // var messages = new ChatMessage[]
        // {
        //     new SystemMessage("Ești un generator de SQL."),
        //     new UserMessage("Schema bazei de date:\n" + schema),
        //     new UserMessage("Scrie o interogare SQL care selectează clienții cu vârsta peste 30")
        // };
        // var messages = new List<ChatMessage>();
        // messages.Add(new SystemChatMessage("esti un generator de sql"));
        // messages.Add(new UserChatMessage("Schema bazei de date:\n" + schema));
        // messages.Add(new UserChatMessage(userRequest));
        //
        

        // Folosește metoda corectă: CompleteChatAsync
        var completion = await _client.CompleteChatAsync(messages);

        Console.WriteLine("Răspuns de la AI:");
        // Console.WriteLine(completion.Value.Content);

        return completion.Value.Content[0].Text;
    }

    public async Task<string> InitOPenAiChatDb(string userRequest, string schema)
    {
        // var messages = new ChatMessage[]
        // {
        //     new SystemMessage("Ești un generator de SQL."),
        //     new UserMessage("Schema bazei de date:\n" + schema),
        //     new UserMessage("Scrie o interogare SQL care selectează clienții cu vârsta peste 30")
        // };
        var messages = new List<ChatMessage>();
        messages.Add(new SystemChatMessage("esti un generator de sql"));
        messages.Add(new UserChatMessage("Schema bazei de date:\n" + schema));
        messages.Add(new UserChatMessage(userRequest));
        
        

        // Folosește metoda corectă: CompleteChatAsync
        var completion = await _client.CompleteChatAsync(messages);

        Console.WriteLine("Răspuns de la AI:");
        // Console.WriteLine(completion.Value.Content);

        return completion.Value.Content[0].Text;
    }
    
    public async Task<string> AskOpenAI(string userRequest)
    {
        var messages = new List<ChatMessage>();
        messages.Add(new UserChatMessage(userRequest));
        
        // Folosește metoda corectă: CompleteChatAsync
        var completion = await _client.CompleteChatAsync(messages);

        Console.WriteLine("Răspuns de la AI:");
        // Console.WriteLine(completion.Value.Content);

        return completion.Value.Content[0].Text;
    }
}
