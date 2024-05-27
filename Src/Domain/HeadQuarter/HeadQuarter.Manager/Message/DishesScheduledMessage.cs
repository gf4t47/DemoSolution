// Copyright (c) Demo.
namespace HeadQuarter.Message;

using Communication;
using Domain.Message;

public class DishesScheduledMessage(DishesScheduled payload) : JsonMessage<DishesScheduled>(payload);
