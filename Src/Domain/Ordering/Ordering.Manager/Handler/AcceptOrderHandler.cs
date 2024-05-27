// Copyright (c) Demo.
namespace Ordering.Handler;

using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using Ordering.Command;
using Ordering.Message;
public class AcceptOrderHandler(IMessageSender<OrderApproved> sender) : ICommandHandler<AcceptOrder>
{
    private IMessageSender<OrderApproved> Sender { get; } = sender;

    public async Task<bool> Process(AcceptOrder command)
    {
        var payload = new OrderApproved();
        var msg = new OrderApprovedMessage(payload);
        var response = await this.Sender.Publish(msg).ConfigureAwait(false);
        return response.Type == ResponseType.Ack;
    }
}
