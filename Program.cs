using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;

var config = new OllamaConfig
{
    Endpoint = "http://localhost:11434",
    TextModel = new OllamaModelConfig("deepseek-r1:1.5b", 131072),
    EmbeddingModel = new OllamaModelConfig("deepseek-r1:1.5b", 2048),
};

var memory = new KernelMemoryBuilder()
    .WithOllamaTextGeneration(config)
    .WithOllamaTextEmbeddingGeneration(config)
    .Build<MemoryServerless>();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Importing and processing  document, please wait...");
Console.ResetColor();

await memory.ImportDocumentAsync("budget.txt", documentId: "DOC001");

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("Model is ready to take questions\n");

while (await memory.IsDocumentReadyAsync("DOC001"))
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("... Ask your questions....\n");
    Console.ResetColor();

    var question = Console.ReadLine();
    var answer = await memory.AskAsync(question);

    Console.WriteLine(answer.Result);

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("\n Sourcess: ");
    foreach (var source in answer.RelevantSources)
    {
        Console.WriteLine($" {source.SourceName} - {source.SourceUrl} - {source.Link}");
    }
    Console.ResetColor();
}
