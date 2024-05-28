// Copyright (c) Demo.
namespace Delivery.Command;

using Core.Command;
using Domain.Model;

public record PlanDeliveryData(Customer Customer, Address DeliveryAddress)
{
    public Customer Customer { get; } = Customer;    
    public Address DeliveryAddress { get; } = DeliveryAddress;

}

public class PlanDelivery(PlanDeliveryData data) : IDemoCommand<PlanDeliveryData>
{
    public PlanDeliveryData Data { get; } = data;
}
