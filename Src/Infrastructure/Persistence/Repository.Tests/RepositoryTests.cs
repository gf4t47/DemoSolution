﻿namespace Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
[TestClass]
public class RepositoryTests
{
    private const int RandomDataRange = 3;

    public record Testee(int Id, string Name) : IEntity
    {
        public int Id { get; } = Id;
        public string Name { get; } = Name;
    }

    private static string IdToName(int id)
    {
        return $"Name_{id}";
    }

    private static IEnumerable<Testee> Single
    {
        get
        {
            for (var i = 0; i <= 10; i++)
            {
                yield return new Testee(i, IdToName(i));
            }
        }
    }

    private static IEnumerable<object[]> GetByIdFound
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count());
                yield return [Single, id, IdToName(id)];       
            }
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(GetByIdFound))]
    public async Task TestGetById_Found(IEnumerable<Testee> existed, int id, string expectedName)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        var actual = await repo.GetById(id).ConfigureAwait(false);
        Assert.AreEqual(expectedName, actual.Name);
    }

    private static IEnumerable<object[]> GetByIdNotFound
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count());
                yield return [Single, id + Single.Count()];       
            }
        }
    }

    private static IEnumerable<object[]> GetAll
    {
        get
        {
            yield return [Single];
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(GetAll))]
    public async Task TestGetAll(IEnumerable<Testee> existed)
    {
        var testees = existed.ToList();
        IRepository<Testee> repo = new MemoryRepository<Testee>(testees);
        var list = (await repo.GetAll().ConfigureAwait(false)).ToList();
        Assert.IsNotNull(list);
        Assert.AreEqual(list.Count(), testees.Count);
        foreach (var found in list.Select(actual => testees.FirstOrDefault(t => t.Id == actual.Id && t.Name == actual.Name)))
        {
            Assert.IsNotNull(found);
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(GetByIdNotFound))]
    public async Task TestGetById_NotFound(IEnumerable<Testee> existed, int id)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            async () => await repo.GetById(id).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private static IEnumerable<object[]> TestAddSucceed
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count()) + Single.Count();
                yield return [Single, new Testee(id, IdToName(id))];       
            }   
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(TestAddSucceed))]
    public async Task TestAdd_Succeed(IEnumerable<Testee> existed, Testee newEntity)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                async () => await repo.GetById(newEntity.Id).ConfigureAwait(false))
            .ConfigureAwait(false);
        
        var succeed = await repo.Add(newEntity).ConfigureAwait(false);
        Assert.IsTrue(succeed);

        var found = await repo.GetById(newEntity.Id).ConfigureAwait(false);
        Assert.IsNotNull(found);
        Assert.AreEqual(newEntity.Name, found.Name);
    }
    
    private static IEnumerable<object[]> TestAddFailed
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count());
                yield return [Single, new Testee(id, IdToName(id))];       
            }
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestAddFailed))]
    public async Task TestAdd_Failed(IEnumerable<Testee> existed, Testee newEntity)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        var succeed = await repo.Add(newEntity).ConfigureAwait(false);
        Assert.IsFalse(succeed);

        var found = await repo.GetById(newEntity.Id).ConfigureAwait(false);
        Assert.IsNotNull(found);
    }

    private static IEnumerable<object[]> TestUpdateSucceed
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count());
                yield return [Single, new Testee(id, IdToName(id + Single.Count()))];       
            }
        }        
    }

    [DataTestMethod]
    [DynamicData(nameof(TestUpdateSucceed))]
    public async Task TestUpdate_Succeed(IEnumerable<Testee> existed, Testee modified)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        var succeed = await repo.Update(modified).ConfigureAwait(false);
        Assert.IsTrue(succeed);

        var found = await repo.GetById(modified.Id).ConfigureAwait(false);
        Assert.IsNotNull(found);
        Assert.AreNotEqual(IdToName(found.Id), found.Name);
        Assert.AreEqual(modified.Name, found.Name);
    }

    private static IEnumerable<object[]> TestUpdateFailed
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count()) + Single.Count();
                yield return [Single, new Testee(id, IdToName(id))];       
            }
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestUpdateFailed))]
    public async Task TestUpdate_Failed(IEnumerable<Testee> existed, Testee modified)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        var succeed = await repo.Update(modified).ConfigureAwait(false);
        Assert.IsFalse(succeed);

        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            async () => await repo.GetById(modified.Id).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private static IEnumerable<object[]> TestDeleteSucceed
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count());
                yield return [Single, id];       
            }            
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(TestDeleteSucceed))]
    public async Task TestDelete_Succeed(IEnumerable<Testee> existed, int toDel)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        var found = await repo.GetById(toDel).ConfigureAwait(false);
        Assert.IsNotNull(found);
        
        var succeed = await repo.Delete(toDel).ConfigureAwait(false);
        Assert.IsTrue(succeed);

        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            async () => await repo.GetById(toDel).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private static IEnumerable<object[]> TestDeleteFailed
    {
        get
        {
            var random = new Random();
            for (var i = 0; i < RandomDataRange; i++)
            {
                var id = random.Next(0, Single.Count()) + Single.Count();
                yield return [Single, id];       
            }
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestDeleteFailed))]
    public async Task TestDelete_Failed(IEnumerable<Testee> existed, int toDel)
    {
        IRepository<Testee> repo = new MemoryRepository<Testee>(existed);
        
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                async () => await repo.GetById(toDel).ConfigureAwait(false))
            .ConfigureAwait(false);
        
        var succeed = await repo.Delete(toDel).ConfigureAwait(false);
        Assert.IsFalse(succeed);
    }    
}
