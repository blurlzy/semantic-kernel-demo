﻿using System.Text.Json.Serialization;

namespace ZL.SemanticKernelDemo.Host.Models
{
    public class ChatSession
    {
        private const string CurrentVersion = "2.0";

        /// <summary>
        /// Chat ID that is persistent and unique.
        /// </summary>
        public string Id { get; set; }

        [JsonIgnore]
        public string Partition => this.Id;

        /// <summary>
        /// unique Id of the user who created this session. (azure object id)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Name of the user who created this message. (azure upn)
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// Title of the chat.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Timestamp of the chat creation.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        ///// <summary>
        ///// System description of the chat that is used to generate responses.
        ///// </summary>
        //public string SystemDescription { get; set; }

        ///// <summary>
        ///// Fixed system description with "TimeSkill" replaced by "TimePlugin"
        ///// </summary>
        //public string SafeSystemDescription => this.SystemDescription.Replace("TimeSkill", "TimePlugin", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// The balance between long term memory and working term memory.
        /// The higher this value, the more the system will rely on long term memory by lowering
        /// the relevance threshold of long term memory and increasing the threshold score of working memory.
        /// </summary>
        public float MemoryBalance { get; set; } = 0.5F;

        /// <summary>
        /// A list of enabled plugins.
        /// </summary>
        public HashSet<string> EnabledPlugins { get; set; } = new();

        /// <summary>
        /// Used to determine if the current chat requires upgrade.
        /// </summary>
        public string? Version { get; set; }

        // soft delete
        public bool IsDeleted { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSession"/> class.
        /// </summary>
        /// <param name="title">The title of the chat.</param>
        /// <param name="systemDescription">The system description of the chat.</param>
        public ChatSession(string title,  string userId, string userName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Title = title;
            this.UserId = userId;
            this.UserName = userName;
            this.CreatedOn = DateTimeOffset.Now;
            //this.SystemDescription = systemDescription;
            this.Version = CurrentVersion;
            this.IsDeleted = false;
        }
    }
}
