namespace Domain;

using Core;
public class Delivery(int id) : IEntity
{
    public int Id { get; } = id;
}
