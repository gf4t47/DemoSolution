// Copyright (c) Demo.
namespace Domain.Message;

using Domain.Model;

public record DeliveryCompleted(int OrderId, Customer Customer, Address DeliveryAddress)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;
    public Address DeliveryAddress { get; } = DeliveryAddress;
}
