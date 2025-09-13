using ef_implementation_tests;
using Npgsql;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using TextToSqlDemo.Services;

namespace ef_gpt;

public class V2: BaseAi
{
    public async Task Run()
        {
            NpgsqlConnection.GlobalTypeMapper.UseVector();
            // 2. Extract schema
            using var db = CreateDataContext();
            string schema = SchemaExtractor.GetDatabaseSchema(db);
            
            var connectionString = "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=embeddingsdb";
            var store = new EmbeddingStore(connectionString);
            await store.EnsureTableAsync();
            
            EmbeddingClient client = new("text-embedding-3-small", KEYS.claudiu_org_key);
            // 3. Setup SQL generator (AI)
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? KEYS.api_key;
            var sqlGen = new SqlGenerator(apiKey);
            
            // await EmbedDirectory(client, store, "ef-base-repository");
            // return;

            // return;
            // 3. Setup SQL generator (AI)
           
            Console.WriteLine("=== Query Console ===");
            Console.WriteLine("Scrie o întrebare (ex: 'Afișează clienții cu vârsta > 30')");
            Console.WriteLine("Tastează 'exit' pentru a ieși.\n");

            var count = 0;
            
            var messages = new List<ChatMessage>();
            // messages.Add(new UserChatMessage("Schema bazei de date:\n" + schema));

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
                
                // messages.Add(new UserChatMessage(request));
                try
                {
                    string response = "";
                    // 4. Get SQL from AI
                    response = await AskOpenAi(store, client, sqlGen, request);
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

    public async Task<string> AskOpenAi(EmbeddingStore store, EmbeddingClient client, SqlGenerator openAiMethods, string userQuery, int topK = 5)
    {
        // 1️⃣ Generate embedding for the query
       OpenAIEmbedding embeddingResponse = await client.GenerateEmbeddingAsync(userQuery);
        ReadOnlyMemory<float> vector = embeddingResponse.ToFloats();
        var queryVector = vector.ToArray();
        
        // 2️⃣ Retrieve top-K relevant embeddings from Postgres
        var topResults = await store.SearchSimilarAsync(queryVector, topK);

        // 3️⃣ Combine retrieved text into context
        var context = string.Join("\n\n", topResults.Select(r =>
            $"File: {r.referenceId}\nContent: {r.metadata}"));

        var messageToOpenAI = $"Here is the context:\n{context}\n\nQuestion: {userQuery}";

        var resp = await openAiMethods.AskOpenAI(messageToOpenAI);
        return resp;
        // 4️⃣ Ask OpenAI Chat to generate code
        // var chatResponse = await _client.ChatEndpoint.GetCompletionAsync(
        //     new OpenAI.Chat.ChatRequest
        //     {
        //         Model = "gpt-4o-mini",
        //         Messages = new[]
        //         {
        //             new OpenAI.Chat.ChatMessage("system", "You are a helpful assistant that writes C# code."),
        //             new OpenAI.Chat.ChatMessage("user", $"Here is the context:\n{context}\n\nQuestion: {userQuery}")
        //         },
        //         MaxTokens = 500
        //     }
        // );
        //
        // return chatResponse.Choices[0].Message.Content;
    }
    
    public async Task EmbedDirectory(EmbeddingClient client,
        EmbeddingStore postgressStore,
        string dirPath)
    {
        try
        {
            string? projectPath = GetProjectPath();
            if (string.IsNullOrEmpty(projectPath))
            {
                return;
            }

            string pathValue = Path.Combine(projectPath, dirPath);
            var files = Directory.GetFiles(pathValue, "*.cs");
            foreach (var file in files)
            {
                var text = await File.ReadAllTextAsync(file);

                // return;
                // get embedding from OpenAI
                OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(text);
                ReadOnlyMemory<float> vector = embedding.ToFloats();
                var arr = vector.ToArray();


                await postgressStore.InsertEmbeddingAsync(
                    Path.GetFileName(file),
                    embedding: arr,
                    new { FilePath = file }
                );

                Console.WriteLine($"Inserted embedding for {file}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}