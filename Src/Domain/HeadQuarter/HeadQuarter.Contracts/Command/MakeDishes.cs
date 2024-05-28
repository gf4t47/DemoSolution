// Copyright (c) Demo.
namespace HeadQuarter.Command;

using System.Collections.Generic;
using Core.Command;
using Domain.Model;

public record MakeDishedData(int OrderId, Customer Customer, ICollection<Dishes> Food)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;
    public ICollection<Dishes> Food { get; } = Food;
}

public class MakeDishes(MakeDishedData data) : IDemoCommand<MakeDishedData>
{
    public MakeDishedData Data { get; } = data;
}
