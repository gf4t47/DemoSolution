// Copyright (c) Demo.
namespace HeadQuarter.Handler;

using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Message;
public class MakeDishesHandler(IMessageSender<DishesScheduled> sender) : ICommandHandler<MakeDishes>
{
    private IMessageSender<DishesScheduled> Sender { get; } = sender;
    
    public async Task<bool> Process(MakeDishes command)
    {
        var payload = new DishesScheduled();
        var msg = new DishesScheduledMessage(payload);
        var response = await this.Sender.Publish(msg).ConfigureAwait(false);
        return response.Type == ResponseType.Ack;
    }
}
