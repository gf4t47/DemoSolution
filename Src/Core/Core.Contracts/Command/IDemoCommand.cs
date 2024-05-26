// Copyright (c) Demo.
namespace Core.Command;

public interface IDemoCommand;

public interface IDemoCommand<out TData> : IDemoCommand
{
    public TData Data { get; }
}
