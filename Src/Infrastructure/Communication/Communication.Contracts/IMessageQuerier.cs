﻿// Copyright (c) Demo.
namespace Communication;

using System.Threading.Tasks;
public interface IMessageQuerier<T>
{
    Task<IMessage<T>?> Receive();
}
