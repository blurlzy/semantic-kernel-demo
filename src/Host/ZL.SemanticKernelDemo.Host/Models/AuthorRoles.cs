namespace ZL.SemanticKernelDemo.Host.Models
{
    /// <summary>
    /// Role of the author of a chat message.
    /// </summary>
    public enum AuthorRoles
    {
        /// <summary>
        /// The current user of the chat.
        /// </summary>
        User = 0,

        /// <summary>
        /// The bot.
        /// </summary>
        Bot
    }


    public readonly struct MessageAuthorRole 
    {
        /// <summary>
        /// The role that instructs or sets the behavior of the assistant.
        /// </summary>
        public static string System => "system";

        /// <summary>
        /// The role that provides responses to system-instructed, user-prompted input.
        /// </summary>
        public static string Assistant => "assistant";

        /// <summary>
        /// The role that provides input for chat completions.
        /// </summary>
        public static string User => "user";

        /// <summary>
        /// The role that provides additional information and references for chat completions.
        /// </summary>
        public static string Tool => "tool";
    }

}
