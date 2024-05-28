// Copyright (c) Demo.
namespace Workshop.Message;

using Communication;
using Domain.Message;

public class DishReadyMessage(DishesReady payload) : JsonMessage<DishesReady>(payload);
