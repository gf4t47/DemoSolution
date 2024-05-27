// Copyright (c) Demo.
namespace Ordering.Service;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
public class MockPriceCalculator : ICalcPriceService
{
    public Task<int> Calculate(IEnumerable<Dishes> ordered)
    {
        return Task.FromResult(ordered.Count() * 10);
    }
}
