using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZL.SemanticKernelTests
{
    /// <summary>
    /// Information about a single chat message.
    /// </summary>
    public class CopilotChatMessage
    {
        private static readonly JsonSerializerOptions SerializerSettings = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

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

        /// <summary>
        /// Timestamp of the message.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Type of the message.
        /// </summary>
        public ChatMessageType Type { get; set; }

        /// <summary>
        /// Name of the user who sent this message.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Content of the message.
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// Serialize the object to a formatted string.
        /// </summary>
        /// <returns>A formatted string</returns>
        public string ToFormattedString()
        {
            var messagePrefix = $"[{this.Timestamp.ToString("G", CultureInfo.CurrentCulture)}]";
            switch (this.Type)
            {
                case ChatMessageType.Document:
                    //var documentMessage = DocumentMessageContent.FromString(this.Content);
                    //var documentMessageContent = (documentMessage != null) ? documentMessage.ToFormattedString() : "documents";

                    //return $"{messagePrefix} {this.UserName} uploaded: {documentMessageContent}";

                case ChatMessageType.Plan:    // Fall through
                case ChatMessageType.Message:
                    return $"{messagePrefix} {this.UserName} said: {this.Content}";

                default:
                    // This should never happen.
                    throw new InvalidOperationException($"Unknown message type: {this.Type}");
            }
        }
    }
}
