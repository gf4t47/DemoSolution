namespace Repository.Memory;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Contracts;
using Repository.Contracts;

public class MemoryRepository<T>(IEnumerable<T> data) : IRepository<T>
    where T : IEntity
{

    /// <summary>
    /// Data list holds in memory
    /// </summary>
    private List<T> Data { get; } = data.ToList();


    public Task<T> GetById(int id)
    {
        var found = this.Data.FirstOrDefault(it => it.Id == id);
        if (found is null)
        {
            throw new KeyNotFoundException($"Not found id: {id}");
        }

        return Task.FromResult(found);
    }

    public Task<IEnumerable<T>> GetAll()
    {
        return Task.FromResult<IEnumerable<T>>(this.Data);
    }

    public Task<bool> Add(T entity)
    {
        var found = this.Data.FirstOrDefault(it => it.Id == entity.Id);
        if (found is not null)
        {
            return Task.FromResult(false);
        }

        this.Data.Add(entity);
        return Task.FromResult(true);
    }

    public Task<bool> Update(T entity)
    {
        var index = this.Data.FindIndex(it => it.Id == entity.Id);
        if (index < 0)
        {
            return Task.FromResult(false);
        }

        this.Data[index] = entity;
        return Task.FromResult(true);
    }

    public Task<bool> Delete(int id)
    {
        var index = this.Data.FindIndex(it => it.Id == id);
        if (index < 0)
        {
            return Task.FromResult(false);
        }
        
        this.Data.RemoveAt(index);
        return Task.FromResult(true);
    }
}
