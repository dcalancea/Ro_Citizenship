namespace WebApplication1.Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        void Save(TEntity entity);
        void Delete(TEntity entity);
    }
}
