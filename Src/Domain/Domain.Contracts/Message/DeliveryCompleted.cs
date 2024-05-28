﻿// Copyright (c) Demo.
namespace Domain.Message;

using Domain.Model;

public record DeliveryCompleted(Customer Customer, Address DeliveryAddress)
{
    public Customer Customer { get; } = Customer;
    public Address DeliveryAddress { get; } = DeliveryAddress;
}