// Copyright (c) Demo.
namespace Ordering.Command;

using Core.Command;
using Ordering.Model;

public record VerifyData(Order Order)
{
    public Order Order { get; } = Order;
}

public class AcceptOrder(VerifyData data) : IDemoCommand<VerifyData>
{
    public VerifyData Data { get; } = data;
}
