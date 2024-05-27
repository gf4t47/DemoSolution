// Copyright (c) Demo.
namespace HeadQuarter.Internal;

using System.Collections.Generic;
using Domain.Message;
using HeadQuarter.Command;
public static class DtoConversionUtility
{
    public static MakeDishedData ToDishes(this OrderApproved order, IDictionary<string, string> header)
    {
        // todo: pass real data
        return new MakeDishedData();
    }

    public static DeliverDishesData ToDelivery(this OrderApproved order, IDictionary<string, string> header)
    {
        // todo: pass real data
        return new DeliverDishesData();
    }
}
