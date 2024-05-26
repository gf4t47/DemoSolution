// Copyright (c) Demo.
namespace Persistence;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
[TestClass]
public class IdGeneratorTests
{
    public record Testee(int Id, string Name) : IEntity
    {
        public int Id { get; } = Id;
        public string Name { get; } = Name;
    }

    private static string IdToName(int id)
    {
        return $"Name_{id}";
    }

    private static IEnumerable<IEnumerable<Testee>> Data
    {
        get
        {
            for (var i = 0; i <= 10; i++)
            {
                yield return Enumerable.Range(0, i).Select(id => new Testee(id + 1, IdToName(id)));
            }
        }
    }

    private static IEnumerable<object[]> TestGenerateId
    {
        get
        {
            return Data.Select(list => new[] { list });
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestGenerateId))]
    public async Task TestGenerateId_Succeed(IEnumerable<Testee> existed)
    {
        var list = existed.ToList();
        var repo = new MemoryRepository<Testee>(list);
        var nextID = repo.Next();
        Assert.AreEqual(list.Count+1, nextID);

        var count = list.Count * 2 + 1;
        while (count > 0)
        {
            var id = repo.Next();
            var entity = new Testee(id, IdToName(id));
            var ret = await repo.Add(entity).ConfigureAwait(false);
            Assert.IsTrue(ret);
            
            count--;
        }
    }
}
