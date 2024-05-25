// Copyright (c) Demo.
namespace Ordering.Command;

using Core.Command;
public record RejectData(int OrderId)
{
    public int OrderId { get; } = OrderId;
}

public class RejectOrder(RejectData data) : IDemoCommand<RejectData>
{
    public RejectData Data { get; } = data;
}
