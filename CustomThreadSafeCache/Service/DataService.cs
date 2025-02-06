using CustomThreadSafeCache.Datas;
using CustomThreadSafeCache.Entities;
using CustomThreadSafeCache.Interfaces;
using System.Text;
using System.Text.Json;

namespace CustomThreadSafeCache.Service
{
    /// <summary>
    /// DataService is a generic class responsible for managing data entities, 
    /// including fetching, deleting, and saving them. It supports caching 
    /// for improved performance and uses a generic type parameter for flexibility.
    /// </summary>
    public class DataService<TEntity> : FileAccsess, IDataService<TEntity> where TEntity : IEntity
    {
        private ICachingService _cachingService;

        /// <summary>
        /// Constructor initializes the caching service.
        /// </summary>
        public DataService()
        {
            _cachingService = new CachingService();
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="entityId">The ID of the entity to delete.</param>
        public async Task DeleteEntityById(int entityId)
        {
            var AllEntities = await GetAll();

            foreach (var entity in AllEntities)
            {
                if (entity is TEntity DeletingEntity)
                {
                    if (DeletingEntity.Id == entityId)
                    {
                        AllEntities.Remove(entity);

                        // Save the updated list after deletion.
                        await SaveAll(AllEntities);

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all entities of the specific type.
        /// Caches the result to avoid redundant data fetching.
        /// </summary>
        /// <returns>A list of entities of type TEntity.</returns>
        public async Task<IEnumerable<object>> GetAllSpecificEntities()
        {
            var key = $"{nameof(TEntity)}";
            object AllEntities = null;

            // Attempt to fetch from cache.
            if (await _cachingService.TryGetValue<TEntity>(key, out object result))
            {
                AllEntities = result;
            }

            // If not found in cache, retrieve from file.
            if (AllEntities == null)
            {
                AllEntities = await GetAll();
            }

            List<object> SpecificEntity = new List<object>();

            foreach (var entity in (List<object>)AllEntities)
            {
                if (entity is TEntity)
                {
                    SpecificEntity.Add(entity);
                }
            }

            // Cache the result for future use.
            await _cachingService.GetOrSet<TEntity>(key, SpecificEntity);

            return SpecificEntity;
        }

        /// <summary>
        /// Retrieves an entity by its ID from either cache or file.
        /// </summary>
        /// <param name="entityId">The ID of the entity to retrieve.</param>
        /// <returns>The entity if found, otherwise the default value.</returns>
        public async Task<object> GetEntityById(int entityId)
        {
            var key = $"{typeof(TEntity).Name}:{entityId}";

            object Entity = null;

            // Try to get the entity from cache.
            if (await _cachingService.TryGetValue<TEntity>(key, out object result))
            {
                Entity = result;
            }

            // If found in cache, return it.
            if (Entity != null)
            {
                return Entity;
            }

            // Otherwise, fetch the entity from file.
            var AllEntities = await GetAll();

            foreach (var entity in AllEntities)
            {
                if (entity is TEntity DeletingEntity)
                {
                    if (DeletingEntity.Id == entityId)
                    {
                        // Cache the entity for future use.
                        await _cachingService.GetOrSet<TEntity>(key, DeletingEntity);
                        return entity;
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// Adds a new entity to the list and saves it to the file.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public async Task SetDatas(TEntity entity)
        {
            var allEntities = await GetAll();

            allEntities.Add(entity);

            // Save the updated list after adding the new entity.
            await SaveAll(allEntities);
        }

        /// <summary>
        /// Placeholder method for updating an entity by its ID. Not yet implemented.
        /// </summary>
        /// <param name="entityId">The ID of the entity to update.</param>
        public Task UpdateEntityById(int entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves all entities from a JSON file, deserializes them, and returns the list of objects.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public async Task<List<object>> GetAll()
        {
            var jsonArray = await File.ReadAllTextAsync(File_Path);
            var json = jsonArray.ToArray();

            List<object> Entities = new List<object>();

            for (int i = 0; i < json.Length; i++)
            {
                var jsoni = json[i].ToString();
                if (json[i] == '[')
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(json[i].ToString());

                    for (int j = i + 1; j < json.Length; j++)
                    {
                        sb.Append(json[j].ToString());

                        if (json[j] == ']')
                        {
                            // Deserialize based on type of entity
                            if (sb.ToString().Contains("BookId"))
                            {
                                List<Book> books = JsonSerializer.Deserialize<List<Book>>(sb.ToString());
                                Entities.AddRange(books);
                                break;
                            }
                            else if (sb.ToString().Contains("ProductId"))
                            {
                                List<Product> Products = JsonSerializer.Deserialize<List<Product>>(sb.ToString());
                                Entities.AddRange(Products);
                                break;
                            }
                            else if (sb.ToString().Contains("UserId"))
                            {
                                List<User> Users = JsonSerializer.Deserialize<List<User>>(sb.ToString());
                                Entities.AddRange(Users);
                                break;
                            }
                            else if (sb.ToString().Contains("OrderId"))
                            {
                                List<Order> Orders = JsonSerializer.Deserialize<List<Order>>(sb.ToString());
                                Entities.AddRange(Orders);
                                break;
                            }
                        }
                    }
                }
            }

            return Entities;
        }

        /// <summary>
        /// Saves all entities to a file after serializing them into JSON.
        /// </summary>
        /// <param name="entities">The list of entities to save.</param>
        private async Task SaveAll(List<object> entities)
        {
            // Convert each entity to JSON string and save to the file.
            var allLines = entities.Select(e => JsonSerializer.Serialize(e)).ToArray();
            await File.WriteAllLinesAsync(File_Path, allLines);
        }
    }
}
