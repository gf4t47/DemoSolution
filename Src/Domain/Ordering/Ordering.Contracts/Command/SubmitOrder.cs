// Copyright (c) Demo.
namespace Ordering.Command;

using System.Collections.Generic;
using Core.Command;
using Domain.Model;
using Ordering.Model;

public record SubmitData(Customer Customer, ICollection<Dishes> Food, PaymentInfo Payment)
{
    public Customer Customer{ get; } = Customer;
    public ICollection<Dishes> Food { get; } = Food;
    public PaymentInfo Payment { get; } = Payment;
    
    public Address? Destination { get; set; }
}

public class SubmitOrder(SubmitData data) : IDemoCommand<SubmitData>
{
    public SubmitData Data { get; } = data;
}
