using ef_implementation_tests;
using OpenAI.Chat;
using OpenAI.Embeddings;
using TextToSqlDemo.Services;

namespace ef_gpt;

public class V1: BaseAi
{
    public async Task Run()
        {
            // 2. Extract schema
            using var db = CreateDataContext();
            string schema = SchemaExtractor.GetDatabaseSchema(db);
            
            // 3. Setup SQL generator (AI)
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? KEYS.api_key;
            var sqlGen = new SqlGenerator(apiKey);

            Console.WriteLine("=== Query Console ===");
            Console.WriteLine("Scrie o întrebare (ex: 'Afișează clienții cu vârsta > 30')");
            Console.WriteLine("Tastează 'exit' pentru a ieși.\n");

            var count = 0;
            
            var messages = new List<ChatMessage>();
            messages.Add(new UserChatMessage("Schema bazei de date:\n" + schema));

            while (true)
            {
                Console.Write("\n>> ");
                string? request = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(request)) continue;
                if (request.ToLower().Trim() == "exit") break;

                // c:ef-base-repository/GenericRepository.cs
                if (request.StartsWith("c:"))
                {
                    var command = request.Split(":");
                    try
                    {
                        string text = GetTextFromFile(command[1]);
                        messages.Add(new UserChatMessage("code:\n" + text));
                        // /Users/claudiugombos/work/portofolio/ef-dapper/ef-dapper/ef-base-repository/GenericRepository.cs
                        Console.WriteLine("\n>> " + command[1] + "\n" + "context updated");
                        continue;
                    }
                    catch (Exception e)
                    {
                        // c:ef-base-repository/GenericRepository.cs
                        Console.WriteLine(e);
                        continue;
                    }
                     
                }
                
                messages.Add(new UserChatMessage(request));
                try
                {
                    string response = "";
                    // 4. Get SQL from AI
                    response = await sqlGen.InitOPenAiChatDb(request, schema);
                    messages.Add(new AssistantChatMessage(response));

                    Console.WriteLine($"\nSQL generat:\n{response}");

                    // 5. Execute query
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
}