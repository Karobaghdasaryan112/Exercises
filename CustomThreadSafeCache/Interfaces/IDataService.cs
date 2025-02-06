
namespace CustomThreadSafeCache.Interfaces
{
    public interface IDataService<TEntity>
    {
        Task SetDatas(TEntity entity);

        Task<object> GetEntityById(int entityId);

        Task DeleteEntityById(int entityId);

        Task UpdateEntityById(int entityId);

        Task<IEnumerable<object>> GetAllSpecificEntities();

    }
}
