// Copyright (c) Demo.
namespace HeadQuarter.Internal;

using System.Collections.Generic;
using Domain.Message;
using HeadQuarter.Command;
public static class DtoConversionUtility
{
    public static MakeDishedData ToDishes(this OrderApproved order, IDictionary<string, string> header)
    {
        return new MakeDishedData(order.OrderId, order.Customer, order.Food);
    }

    public static DeliverDishesData ToDelivery(this OrderApproved order, IDictionary<string, string> header)
    {
        return new DeliverDishesData(order.OrderId, order.Customer, order.DeliveryAddress);
    }
}
