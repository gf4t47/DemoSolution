namespace Persistence;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
public class MemoryRepository<T>(IEnumerable<T> data) : IRepository<T>, IUniqueIdGenerator<T>
    where T : IEntity
{

    public MemoryRepository() : this(new List<T>())
    {
    }

    /// <summary>
    /// Thread-safe Dictionary that holds data in memory
    /// </summary>
    private ConcurrentDictionary<int, T> Data { get; } = new(data.Select(it => new KeyValuePair<int, T>(it.Id, it)));

    private int MaxId { get; set; }

    public Task<T> GetById(int id)
    {
        if (this.Data.TryGetValue(id, out var found))
        {
            return Task.FromResult(found);    
        }
        
        throw new KeyNotFoundException($"Not found id: {id}");
    }

    public Task<IEnumerable<T>> GetAll()
    {
        return Task.FromResult<IEnumerable<T>>(this.Data.Values);
    }

    public Task<bool> Add(T entity)
    {
        var ret = this.Data.TryAdd(entity.Id, entity);
        if (ret)
        {
            this.MaxId = Math.Max(this.MaxId, entity.Id);
        }
        
        return Task.FromResult(ret);
    }

    public Task<bool> Update(T entity)
    {
        return Task.FromResult(this.Data.TryGetValue(entity.Id, out var original) 
            && this.Data.TryUpdate(entity.Id, entity, original));
    }

    public Task<bool> Delete(int id)
    {
        return Task.FromResult(this.Data.TryRemove(id, out _));
    }

    public int Next()
    {
        return this.MaxId + 1;
    }
}
