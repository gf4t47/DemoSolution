// Copyright (c) Demo.
namespace HeadQuarter.Command;

using Core.Command;

public record MakeDishedData();

public class MakeDishes(MakeDishedData data) : IDemoCommand<MakeDishedData>
{
    public MakeDishedData Data { get; } = data;
}
