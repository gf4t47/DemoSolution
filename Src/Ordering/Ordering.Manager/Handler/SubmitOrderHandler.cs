// Copyright (c) Demo.
namespace Ordering.Handler;

using System.Threading.Tasks;
using Core.Command;
using Ordering.Command;
using Ordering.Model;
using Ordering.Service;
public class SubmitOrderHandler(OrderingCommandBus commandBus, ICalcPriceService calcPriceService, IPaymentService paymentService) : ICommandHandler<SubmitOrder>
{
    private OrderingCommandBus CommandBus { get; } = commandBus;
    private ICalcPriceService CalcPriceService { get; } = calcPriceService;
    private IPaymentService PaymentService { get; } = paymentService;

    public async Task<bool> Process(SubmitOrder command)
    {
        var submit = command.Data.Order;
        var prices = await this.CalcPriceService.Calculate(submit.Food).ConfigureAwait(false);
        var paid = await this.PaymentService.Pay(prices, submit.PaymentInfo).ConfigureAwait(false);

        if (paid)
        {
            await this.ApprovePayment(submit).ConfigureAwait(false);
        }
        else
        {
            await this.RejectPayment(submit).ConfigureAwait(false);
        }

        return true;
    }

    private Task<bool> ApprovePayment(Order submit)
    {
        var accept = new VerifyData(submit);
        return this.CommandBus.Execute(new AcceptOrder(accept));        
    }

    private Task<bool> RejectPayment(Order submit)
    {
        var reject = new RejectData(submit);
        return this.CommandBus.Execute(new RejectOrder(reject));        
    }
}
