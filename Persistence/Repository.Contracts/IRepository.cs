namespace Repository.Contracts;

using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Contracts;
public interface IRepository<T> where T : IEntity
{
    Task<T> GetById(int id);

    Task<IEnumerable<T>> GetAll();

    Task<bool> Add(T entity);

    Task<bool> Update(T entity);

    Task<bool> Delete(int id);
}
