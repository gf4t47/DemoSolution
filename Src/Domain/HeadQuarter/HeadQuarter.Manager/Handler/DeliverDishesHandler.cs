// Copyright (c) Demo.
namespace HeadQuarter.Handler;

using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Message;
public class DeliverDishesHandler(IMessageSender<DeliveryScheduled> sender) : ICommandHandler<DeliverDishes>
{
    private IMessageSender<DeliveryScheduled> Sender { get; } = sender;

    public async Task<bool> Process(DeliverDishes command)
    {
        var payload = new DeliveryScheduled();
        var msg = new DeliveryScheduledMessage(payload);
        var response = await this.Sender.Publish(msg).ConfigureAwait(false);
        return response.Type == ResponseType.Ack;
    }
}
