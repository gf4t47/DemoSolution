// Copyright (c) Demo.
namespace Core;

using System.Threading.Tasks;
using Core.Command;
public interface ICommandBusT
{
    Task<bool> Execute<TInput>(IDemoCommand<TInput> command);
}
