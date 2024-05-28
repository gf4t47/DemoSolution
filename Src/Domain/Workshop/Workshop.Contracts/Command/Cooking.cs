// Copyright (c) Demo.
namespace Workshop.Command;

using System.Collections.Generic;
using Core.Command;
using Domain.Model;
public record CookingData(int OrderId, Customer Customer, ICollection<Dishes> ToCook)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;
    public ICollection<Dishes> ToCook { get; } = ToCook;
}

public class Cooking(CookingData data) : IDemoCommand<CookingData>
{
    public CookingData Data { get; } = data;
}
