// Copyright (c) Demo.
namespace Ordering.Handler;

using System.Threading.Tasks;
using Core.Command;
using Ordering.Command;
public class CancelOrderHandler : ICommandHandler<CancelOrder>
{

    public Task<bool> Process(CancelOrder command)
    {
        throw new System.NotImplementedException();
    }
}
