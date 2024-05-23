// Copyright (c) Demo.
namespace Communication.Contracts;

using System.Threading.Tasks;
public interface IMessageQuerier
{
    Task<IMessage<T>?> Receive<T>();
}
