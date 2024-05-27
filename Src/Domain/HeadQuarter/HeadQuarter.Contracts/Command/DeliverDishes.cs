// Copyright (c) Demo.
namespace HeadQuarter.Command;

using Core.Command;

public record DeliverDishesData();

public class DeliverDishes(DeliverDishesData data) : IDemoCommand<DeliverDishesData>
{
    public DeliverDishesData Data { get; } = data;
}
