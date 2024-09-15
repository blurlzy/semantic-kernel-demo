﻿
namespace ZL.SemanticKernelTests
{
    public class PromptsOptions
    {
        // the token (output token) limit of the chat model
        public const int CompletionTokenLimit = 4096;

        //  the token count left for the model to generate text after the prompt.
        public const int ResponseTokenLimit = 1024;

    }
}
