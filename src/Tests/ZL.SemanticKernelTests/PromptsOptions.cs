
namespace ZL.SemanticKernelTests
{
    public class PromptsOptions
    {
        // the token limit of the chat model
        public static int CompletionTokenLimit = 4096;

        //  the token count left for the model to generate text after the prompt.
        public static  int ResponseTokenLimit = 1024;

        public static double ResponseTemperature { get; } = 0.7;
        public static double ResponseTopP { get; } = 1;
        public static double ResponsePresencePenalty { get; } = 0.5;
        public static double ResponseFrequencyPenalty { get; } = 0.5;

        public static double IntentTemperature { get; } = 0.7;
        public static double IntentTopP { get; } = 1;
        public static double IntentPresencePenalty { get; } = 0.5;
        public static double IntentFrequencyPenalty { get; } = 0.5;

        public static string SystemIntentExtraction => string.Join("\n", SystemIntentPromptComponents);

        public static string[] SystemIntentPromptComponents => new string[]
        {
                        SystemDescription,
                        SystemIntent,
                        "{{ChatPlugin.ExtractChatHistory}}",
                        SystemIntentContinuation
        };
        

        public static string SystemDescription { get; set; } = string.Empty;
        public static string SystemIntent { get; set; } = string.Empty;
        public static string SystemIntentContinuation { get; set; } = string.Empty;


    }
}
