// Copyright (c) Demo.
namespace Ordering.Command;

using Core.Command;
using Ordering.Model;

public record RejectData(Order Order)
{
    public Order Order { get; } = Order;
}

public class RejectOrder(RejectData data) : IDemoCommand<RejectData>
{
    public RejectData Data { get; } = data;
}
