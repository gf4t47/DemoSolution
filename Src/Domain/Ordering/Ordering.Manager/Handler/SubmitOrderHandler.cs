// Copyright (c) Demo.
namespace Ordering.Handler;

using System.Threading.Tasks;
using Core.Command;
using Ordering.Command;
using Ordering.Model;
using Ordering.Service;
using Persistence;

/// <summary>
/// submit order command handler
/// </summary>
/// <param name="commandBus">command pipeline to exec new command.</param>
/// <param name="calcPriceService">calculate dishes price, discount, etc.</param>
/// <param name="paymentService">make payment.</param>
/// <param name="repository">persistence layer entry.</param>
/// <param name="generator">make sure new id is unique</param>
public class SubmitOrderHandler(OrderingCommandBus commandBus, 
    ICalcPriceService calcPriceService, 
    IPaymentService paymentService, 
    IRepository<Order> repository, 
    IUniqueIdGenerator<Order> generator) : ICommandHandler<SubmitOrder>
{
    private OrderingCommandBus CommandBus { get; } = commandBus;
    private ICalcPriceService CalcPriceService { get; } = calcPriceService;
    private IPaymentService PaymentService { get; } = paymentService;
    private IRepository<Order> Repository { get; } = repository;
    private IUniqueIdGenerator<Order> Generator { get; } = generator;

    public async Task<bool> Process(SubmitOrder command)
    {
        var submit = command.Data;
        var order = await this.NewEntity(submit).ConfigureAwait(false);
        
        var prices = await this.CalcPriceService.Calculate(submit.Food).ConfigureAwait(false);
        var paid = await this.PaymentService.Pay(prices, submit.Payment).ConfigureAwait(false);

        if (paid)
        {
            await this.ApprovePayment(order).ConfigureAwait(false);
        }
        else
        {
            await this.RejectPayment(order).ConfigureAwait(false);
        }

        return true;
    }

    private async Task<Order> NewEntity(SubmitData data)
    {
        var id = this.Generator.Next();
        var entity = new Order(id, data.Customer, data.Food, data.Payment, data.Destination ?? data.Customer.Address) { Status = Order.StatusSubmit };
        await this.Repository.Add(entity).ConfigureAwait(false);
        return entity;
    }

    private async Task<bool> ApprovePayment(Order order)
    {
        var accept = new VerifyData(order);
        var ret = await this.CommandBus.Execute(new AcceptOrder(accept)).ConfigureAwait(false);
        
        order.Status = Order.StatusApproved;
        await this.Repository.Update(order).ConfigureAwait(false);
        
        return ret;
    }

    private async Task<bool> RejectPayment(Order order)
    {
        var reject = new RejectData(order.Id);
        var ret = await this.CommandBus.Execute(new RejectOrder(reject)).ConfigureAwait(false);

        order.Status = Order.StatusReject;
        await this.Repository.Update(order).ConfigureAwait(false);

        return ret;
    }
}
