using ef_implementation_tests;

namespace ef_gpt;


public class BaseAi: BaseTest
{
    public string GetProjectPath()
    {
        string basePath = AppContext.BaseDirectory;
        string? projectPath = Directory.GetParent(basePath) // net8.0
            ?.Parent // Debug
            ?.Parent // bin
            ?.Parent
            ?.Parent?.FullName;

        return projectPath;
    }

    public string GetTextFromFile(string filename)
    {
        string basePath = AppContext.BaseDirectory;
        string? projectPath = GetProjectPath();
        if (string.IsNullOrEmpty(projectPath))
        {
            return "";
        }

        string filePath = Path.Combine(projectPath, filename);
        string text = File.ReadAllText(filePath);
        return text;
    }

    public async Task<List<(string file, string code)>> GetChunks(string startingPath)
    {
        var codeFiles = Directory.GetFiles(startingPath, "*.cs", SearchOption.AllDirectories);
        var chunks = new List<(string file, string code)>();

        foreach (var file in codeFiles)
        {
            var text = await File.ReadAllTextAsync(file);
            int chunkSize = 500; // lines per chunk
            var lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i += chunkSize)
            {
                var chunk = string.Join("\n", lines.Skip(i).Take(chunkSize));
                chunks.Add((file, chunk));
            }
        }

        return chunks;
    }

   
}