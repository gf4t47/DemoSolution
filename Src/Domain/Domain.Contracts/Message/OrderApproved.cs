// Copyright (c) Demo.
namespace Domain.Message;

using System.Collections.Generic;
using Domain.Model;

public record OrderApproved(int OrderId, Customer Customer, ICollection<Dishes> Food, Address DeliveryAddress)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;
    public ICollection<Dishes> Food { get; } = Food;
    public Address DeliveryAddress { get; } = DeliveryAddress;
}
