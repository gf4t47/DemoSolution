// Copyright (c) Demo.
namespace Core.Command;

using System.Threading.Tasks;
public interface ICommandHandler<in TCommand> where TCommand : IDemoCommand
{
    Task<bool> Process(TCommand command);
}
