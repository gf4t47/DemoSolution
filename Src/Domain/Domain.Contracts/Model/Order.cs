namespace Ordering.Model;

using System.Collections.Generic;
using Core;
using Domain.Model;

public record PaymentInfo;

public class Order(int id, Customer customer, ICollection<Dishes> food, PaymentInfo paymentInfo, Address deliveryAddress) : IEntity
{
    public const string StatusSubmit = nameof(StatusSubmit);

    public const string StatusReject = nameof(StatusReject);

    public const string StatusApproved = nameof(StatusApproved);

    public const string StatusCanceled = nameof(StatusCanceled);
    
    public const string StatusDishesScheduled = nameof(StatusDishesScheduled);

    public const string StatusDishesReady = nameof(StatusDishesReady);

    public const string StatusDeliveryCompleted = nameof(StatusDeliveryCompleted);

    public const string StatusCompleted = nameof(StatusCompleted);
    
    public int Id { get; } = id;
    
    public string Status { get; set; } = StatusSubmit;

    public Customer Customer { get; } = customer;
    
    public Address DeliveryAddress { get; set; } = deliveryAddress;
    
    public PaymentInfo PaymentInfo { get; set; } = paymentInfo;    
    
    public ICollection<Dishes> Food { get; } = food;
}
