// Copyright (c) Demo.
namespace Ordering.Handler;

using System.Threading.Tasks;
using Core.Command;
using Ordering.Command;
public class AcceptOrderHandler : ICommandHandler<AcceptOrder>
{
    public Task<bool> Process(AcceptOrder command)
    {
        // todo: communicate to head quarter
        return Task.FromResult(false);
    }
}
