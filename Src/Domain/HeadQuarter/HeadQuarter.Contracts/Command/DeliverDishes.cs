// Copyright (c) Demo.
namespace HeadQuarter.Command;

using Core.Command;
using Domain.Model;

public record DeliverDishesData(Customer Customer, Address DeliveryAddress)
{
    public Customer Customer { get; } = Customer;
    public Address DeliveryAddress { get; } = DeliveryAddress;
}

public class DeliverDishes(DeliverDishesData data) : IDemoCommand<DeliverDishesData>
{
    public DeliverDishesData Data { get; } = data;
}
