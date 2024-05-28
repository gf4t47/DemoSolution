// Copyright (c) Demo.
namespace Domain.Message;

using System.Collections.Generic;
using Domain.Model;

public record DishesReady(int OrderId, Customer Customer, ICollection<Dishes> Food, Address ShopAddress)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;
    public ICollection<Dishes> Food { get; } = Food;
    
    public Address ShopAddress { get; } = ShopAddress;
}
