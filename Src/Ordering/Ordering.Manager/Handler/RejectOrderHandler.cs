// Copyright (c) Demo.
namespace Ordering.Handler;

using System.Threading.Tasks;
using Core.Command;
using Ordering.Command;
public class RejectOrderHandler : ICommandHandler<RejectOrder>
{
    public Task<bool> Process(RejectOrder command)
    {
        // notify front-end | app | other terminal that payment failed
        return Task.FromResult(true);
    }
}
