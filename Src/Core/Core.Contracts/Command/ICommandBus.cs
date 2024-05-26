// Copyright (c) Demo.
namespace Core.Command;

using System.Threading.Tasks;
public interface ICommandBus
{
    Task<bool> Execute<TInput>(IDemoCommand<TInput> command);
}
