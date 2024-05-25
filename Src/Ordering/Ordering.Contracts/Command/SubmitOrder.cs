// Copyright (c) Demo.
namespace Ordering.Command;

using Core.Command;
using Ordering.Model;

public record SubmitData(Order Order)
{
    public Order Order { get; } = Order;
}

public class SubmitOrder(SubmitData data) : IDemoCommand<SubmitData>
{
    public SubmitData Data { get; } = data;
}
