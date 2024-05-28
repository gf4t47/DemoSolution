// Copyright (c) Demo.
namespace Delivery.Command;

using Core.Command;
using Domain.Model;

public record PlanDeliveryData(int OrderId, Customer Customer, Address DeliveryAddress)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get; } = Customer;    
    public Address DeliveryAddress { get; } = DeliveryAddress;
}

public class PlanDelivery(PlanDeliveryData data) : IDemoCommand<PlanDeliveryData>
{
    public PlanDeliveryData Data { get; } = data;
}
