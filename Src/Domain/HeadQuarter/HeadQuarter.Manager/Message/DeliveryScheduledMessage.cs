// Copyright (c) Demo.
namespace HeadQuarter.Message;

using Communication;
using Domain.Message;

public class DeliveryScheduledMessage(DeliveryScheduled payload) : JsonMessage<DeliveryScheduled>(payload);
