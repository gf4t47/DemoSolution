// Copyright (c) Demo.
namespace Ordering.Service;

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model;
public interface ICalcPriceService
{
    Task<int> Calculate(IEnumerable<Dishes> ordered);
}
