namespace Domain;

using Core;

public abstract record PaymentInfo;

public class Order(int id) : IEntity
{
    public int Id { get; } = id;
    
    public PaymentInfo? PaymentInfo { get; set; }
}
