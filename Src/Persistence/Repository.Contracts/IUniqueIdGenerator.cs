// Copyright (c) Demo.
namespace Persistence;

using Core;
public interface IUniqueIdGenerator<T> where T: IEntity
{
    int Next();
}
