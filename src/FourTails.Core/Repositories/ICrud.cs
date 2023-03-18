namespace FourTails.Core.Repositories;

public interface ICrudRepository<T> where T : class
{
    Task Create(T entity);
    Task<T> ReadById(int id);
    Task<IEnumerable<T>> ReadAll();
    Task Update(T entity);
    Task Delete(T entity);
}