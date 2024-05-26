// Copyright (c) Demo.
namespace Ordering.Command;

using Core.Command;

public record CancelData;

public class CancelOrder(CancelData data) : IDemoCommand<CancelData>
{
    public CancelData Data { get; } = data;
}
