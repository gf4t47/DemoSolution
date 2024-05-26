// Copyright (c) Demo.
namespace Ordering.Model;

using Core;
public class Customer(int id, string fullName, Address address) : IEntity
{
    public int Id { get; } = id;
    
    public string FullName { get; } = fullName;

    public Address Address { get; } = address;
}
