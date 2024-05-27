// Copyright (c) Demo.
namespace Ordering.Service;

using System;
using System.Threading.Tasks;
using Ordering.Model;

/// <summary>
/// In real, should call on `Strip` or `Paypal`'s API to do payment approvement.
/// Here we just mock one that randomly return `approved` or `reject`
/// </summary>
public class MockPaymentService : IPaymentService
{
    private Random Generator { get; } = new();

    public Task<bool> Pay(int amount, PaymentInfo paymentInfo)
    {
        var num = this.Generator.Next(0 ,100);
        return Task.FromResult(num <= 80);
    }
}
