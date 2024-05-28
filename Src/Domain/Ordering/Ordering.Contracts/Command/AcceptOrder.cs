// Copyright (c) Demo.
namespace Ordering.Command;

using System.Collections.Generic;
using Core.Command;
using Domain.Model;
public record VerifyData(int OrderId, Customer Customer, ICollection<Dishes> Food, Address DeliveryAddress)
{
    public int OrderId { get; } = OrderId;

    public Customer Customer { get; } = Customer;
    public ICollection<Dishes> Food { get; } = Food;
    public Address DeliveryAddress { get; } = DeliveryAddress;
}

public class AcceptOrder(VerifyData data) : IDemoCommand<VerifyData>
{
    public VerifyData Data { get; } = data;
}
