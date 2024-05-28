// Copyright (c) Demo.
namespace HeadQuarter.Command;

using Core.Command;
using Domain.Model;

public record DeliverDishesData(int OrderId, Customer Customer, Address DeliveryAddress)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;
    public Address DeliveryAddress { get; } = DeliveryAddress;
}

public class DeliverDishes(DeliverDishesData data) : IDemoCommand<DeliverDishesData>
{
    public DeliverDishesData Data { get; } = data;
}
