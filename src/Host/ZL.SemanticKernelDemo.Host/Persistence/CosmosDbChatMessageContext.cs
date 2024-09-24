namespace ZL.SemanticKernelDemo.Host.Persistence
{
    public class CosmosDbChatMessageContext :CosmosDbContext<CopilotChatMessage>
    {
        /// <summary>
        /// Initializes a new instance of the CosmosDbCopilotChatMessageContext class.
        /// </summary>
        /// <param name="connectionString">The CosmosDB connection string.</param>
        /// <param name="database">The CosmosDB database name.</param>
        /// <param name="container">The CosmosDB container name.</param>
        public CosmosDbChatMessageContext(string connectionString, string database, string container) :
            base(connectionString, database, container)
        {
        }

        /// <inheritdoc/>
        public Task<IEnumerable<CopilotChatMessage>> QueryEntitiesAsync(Func<CopilotChatMessage, bool> predicate, int skip, int count)
        {
            return Task.Run<IEnumerable<CopilotChatMessage>>(
                    () => this._container.GetItemLinqQueryable<CopilotChatMessage>(true)
                            .Where(predicate).OrderByDescending(m => m.Timestamp).Skip(skip).Take(count).AsEnumerable());
        }
    }
}
