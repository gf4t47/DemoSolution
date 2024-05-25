namespace Ordering.Model;

using System.Collections.Generic;
using Core;

public record PaymentInfo;

public class Order(int id, Customer customer, ICollection<Dishes> food, PaymentInfo paymentInfo, Address deliveryAddress) : IEntity
{
    public int Id { get; } = id;
    
    public Customer Customer { get; } = customer;
    
    public ICollection<Dishes> Food { get; } = food;

    public PaymentInfo PaymentInfo { get; set; } = paymentInfo;

    public Address DeliveryAddress { get; set; } = deliveryAddress;
}
