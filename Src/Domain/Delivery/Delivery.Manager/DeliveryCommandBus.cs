// Copyright (c) Demo.
namespace Delivery;

using System.Threading.Tasks;
using Core;
using Core.Command;
public class DeliveryCommandBus : ICommandBus
{

    public Task<bool> Execute(IDemoCommand command)
    {
        throw new System.NotImplementedException();
    }
}
