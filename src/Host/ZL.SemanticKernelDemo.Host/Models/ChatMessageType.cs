namespace ZL.SemanticKernelDemo.Host.Models
{
    /// <summary>
    /// Type of the chat message.
    /// </summary>
    public enum ChatMessageType
    {
        /// <summary>
        /// A standard message
        /// </summary>
        Message,

        /// <summary>
        /// A message for a Plan
        /// </summary>
        Plan,

        /// <summary>
        /// An uploaded document notification
        /// </summary>
        Document,
    }

}
