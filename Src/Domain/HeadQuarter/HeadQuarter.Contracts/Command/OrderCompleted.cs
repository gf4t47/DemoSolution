// Copyright (c) Demo.
namespace HeadQuarter.Command;

using Core.Command;
using Domain.Model;

public record OrderCompletedData(int OrderId, Customer Customer)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;
}

public class OrderCompleted(OrderCompletedData data) : IDemoCommand<OrderCompletedData>
{
    public OrderCompletedData Data{ get; } = data;
}
