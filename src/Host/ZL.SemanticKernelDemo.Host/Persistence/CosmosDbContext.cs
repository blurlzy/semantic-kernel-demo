using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using System.Net;

namespace ZL.SemanticKernelDemo.Host.Persistence
{
    public class CosmosDbContext<T> where T : class
    {
        // cosmos client
        private readonly CosmosClient _client;

        // cosmos db container
        protected readonly Container _container;

        private const int MAX_LIST_COUNT = 300;
        // ctor
        public CosmosDbContext(string connection, string database, string container)
        {
            // Configure JsonSerializerOptions
            var options = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                },
            };
            this._client = new CosmosClient(connection, options);
            this._container = this._client.GetContainer(database, container);
        }

        public async Task<IEnumerable<T>> QueryEntitiesAsync(Func<T, bool> predicate)
        {
            return await Task.Run<IEnumerable<T>>(
              () => this._container.GetItemLinqQueryable<T>(true).Where(predicate).AsEnumerable());
        }

        public async Task<IEnumerable<T>> ListItemsAsync(Func<T, bool> predicate)
        {
            List<T> results = new List<T>();

            // Get LINQ IQueryable object
            IOrderedQueryable<T> queryable = _container.GetItemLinqQueryable<T>(true);

            // Construct LINQ query
            var matches = queryable.Where(predicate).AsQueryable();

            // Convert to feed iterator
            using FeedIterator<T> linqFeed = matches.ToFeedIterator();

            // async
            while (linqFeed.HasMoreResults && results.Count < MAX_LIST_COUNT)
            {
                foreach (var item in await linqFeed.ReadNextAsync())
                {
                    results.Add(item);
                }
            }

            return results;
        }

        public async Task<T> ReadAsync(string entityId, string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(entityId))
            {
                throw new ArgumentOutOfRangeException(nameof(entityId), "Entity Id cannot be null or empty.");
            }

            try
            {
                var response = await this._container.ReadItemAsync<T>(entityId, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Entity with id {entityId} not found.");
            }
        }

        public async Task<ItemResponse<T>> CreateAsync(T entity, string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), "Partition key cannot be null or empty.");
            }

            return await this._container.CreateItemAsync(entity, new PartitionKey(partitionKey));
        }


        public async Task<ItemResponse<T>> UpsertAsync(T entity, string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), "Partition key cannot be null or empty.");
            }

            return await this._container.UpsertItemAsync(entity, new PartitionKey(partitionKey));
        }

        public async Task DeleteAsync(string id, string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                throw new ArgumentOutOfRangeException(nameof(partitionKey), "Partition key cannot be null or empty.");
            }

            await this._container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._client.Dispose();
            }
        }

   
    }
}
