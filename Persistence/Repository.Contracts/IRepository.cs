﻿namespace Repository.Contracts;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository<T>
{
    Task<T> GetById(int id);

    Task<IEnumerable<T>> GetAll();

    Task Add(T entity);

    Task Update(T entity);

    Task Delete(int id);
}
