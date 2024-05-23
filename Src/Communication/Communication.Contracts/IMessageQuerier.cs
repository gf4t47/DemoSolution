// Copyright (c) Demo.
namespace Communication;

using System.Threading.Tasks;
public interface IMessageQuerier
{
    Task<IMessage<T>?> Receive<T>();
}
