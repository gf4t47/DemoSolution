// Copyright (c) Demo.
namespace Delivery.Command;

using Core.Command;
using Domain.Model;

/// <summary>
/// todo: add DeliveryAddress back
/// </summary>
/// <param name="DeliveryAddress"></param>
public record MakeDeliveryData(Customer Customer)
{
    public Customer Customer { get;} = Customer;
    public Address DeliveryAddress { get; }

}

public class MakeDelivery(MakeDeliveryData data) : IDemoCommand<MakeDeliveryData>
{
    public MakeDeliveryData Data { get; } = data;
}
