## Storage Layer

- Simplified DDD `Repository` Pattern 
- First Implement an in-memory "Repository" to achieve a MVP asap.
- Would consider `ADO.NET` + `SQLite` if have more time work on it.

## Repository<[TEntity](../core/IEntity.md)>

```C#
public interface IRepository<T> where T : IEntity  
{  
    Task<T> GetById(int id);  
  
    Task<IEnumerable<T>> GetAll();  
  
    Task<bool> Add(T entity);  
  
    Task<bool> Update(T entity);  
  
    Task<bool> Delete(int id);  
}
```