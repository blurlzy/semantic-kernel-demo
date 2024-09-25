namespace ZL.SemanticKernelDemo.Host.Services
{
    public class PromptsOptions
    {
        // the token limit of the chat model
        public const int CompletionTokenLimit = 1024 * 5;

        //  the token count left for the model to generate text after the prompt.
        public const int ResponseTokenLimit = 1024;

    }
}
