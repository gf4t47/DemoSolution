// Copyright (c) Demo.
namespace Delivery.Message;

using Communication;
using Domain.Message;

public class DeliveryCompletedMessage(DeliveryCompleted payload) : JsonMessage<DeliveryCompleted>(payload);
