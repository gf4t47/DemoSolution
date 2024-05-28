﻿// Copyright (c) Demo.
namespace Domain.Message;

using System.Collections.Generic;
using Domain.Model;

public record DishesReady(Customer Customer, ICollection<Dishes> Food)
{
    public Customer Customer { get; } = Customer;
    public ICollection<Dishes> Food { get; } = Food;
}