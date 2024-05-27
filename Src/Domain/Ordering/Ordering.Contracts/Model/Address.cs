// Copyright (c) Demo.
namespace Ordering.Model;

public record Address(string Street, string City, string State, int ZipCode)
{
    public string Street { get; } = Street;
    public string City { get; } = City;
    public string State { get; } = State;
    public int ZipCode { get; } = ZipCode;
}
