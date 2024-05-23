namespace Domain;

using Core;
public class Order(int id) : IEntity
{
    public int Id { get; } = id;
}
