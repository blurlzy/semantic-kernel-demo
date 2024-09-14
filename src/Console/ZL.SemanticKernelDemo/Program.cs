// See https://aka.ms/new-console-template for more information


Console.WriteLine("Connecting to Azure Key Vault........");
string endpoint = SecretManager.OpenAIEndpoint;
string key = SecretManager.OpenAIKey;
string deployment = "gpt-4o";


Console.WriteLine("=== Example with Chat Completion (Azure OpenAI) ===");

var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(deployment, endpoint,key)
    .Build();

var chatHistory = new ChatHistory();

// First question without previous context based on uploaded content.
var ask = "How did Emily and David meet?";
chatHistory.AddUserMessage(ask);

// Chat Completion example
//var dataSource = GetAzureSearchDataSource();
//#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
//var promptExecutionSettings = new AzureOpenAIPromptExecutionSettings { AzureChatDataSource = dataSource };
//#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();

// var chatMessage = await chatCompletion.GetChatMessageContentAsync(chatHistory, promptExecutionSettings);
var chatMessage = await chatCompletion.GetChatMessageContentAsync(chatHistory);

var response = chatMessage.Content!;

// Output
// Ask: How did Emily and David meet?
// Response: Emily and David, both passionate scientists, met during a research expedition to Antarctica.
Console.WriteLine($"Ask: {ask}");
Console.WriteLine($"Response: {response}");
Console.WriteLine();

// Chat history maintenance
chatHistory.AddAssistantMessage(response);

// Second question based on uploaded content.
ask = "What are Emily and David studying?";
chatHistory.AddUserMessage(ask);

// Chat Completion Streaming example
Console.WriteLine($"Ask: {ask}");
Console.WriteLine("Response: ");

await foreach (var word in chatCompletion.GetStreamingChatMessageContentsAsync(chatHistory))
{
    Console.Write(word);
}

Console.WriteLine(Environment.NewLine);




///// <summary>
///// Initializes a new instance of the <see cref="AzureSearchChatDataSource"/> class.
///// </summary>
//static AzureSearchChatDataSource GetAzureSearchDataSource()
//{
//    return new AzureSearchChatDataSource
//    {
//        Endpoint = new Uri(TestConfiguration.AzureAISearch.Endpoint),
//        Authentication = DataSourceAuthentication.FromApiKey(TestConfiguration.AzureAISearch.ApiKey),
//        IndexName = TestConfiguration.AzureAISearch.IndexName
//    };
//}