// Copyright (c) Demo.
namespace Domain.Model;

public record Address(string Street, string City, string State, int ZipCode)
{
    public string Street { get; } = Street;
    public string City { get; } = City;
    public string State { get; } = State;
    public int ZipCode { get; } = ZipCode;

    public override string ToString()
    {
        return $"{this.Street} {this.City}, {this.State} {this.ZipCode}";
    }
}
