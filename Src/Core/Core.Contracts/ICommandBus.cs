// Copyright (c) Demo.
namespace Core;

using System.Threading.Tasks;
using Core.Command;
public interface ICommandBus
{
    Task<bool> Execute(IDemoCommand command);
}
