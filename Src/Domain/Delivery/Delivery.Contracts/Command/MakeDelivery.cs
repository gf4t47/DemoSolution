﻿// Copyright (c) Demo.
namespace Delivery.Command;

using Core.Command;
using Domain.Model;

public record MakeDeliveryData(int OrderId, Customer Customer, Address ShopAddress)
{
    public int OrderId { get; } = OrderId;
    public Customer Customer { get;} = Customer;
    
    public Address ShopAddress { get; } = ShopAddress;
}

public class MakeDelivery(MakeDeliveryData data) : IDemoCommand<MakeDeliveryData>
{
    public MakeDeliveryData Data { get; } = data;
}
