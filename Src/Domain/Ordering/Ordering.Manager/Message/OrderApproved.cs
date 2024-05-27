// Copyright (c) Demo.
namespace Ordering.Message;

using Communication;
using Domain.Message;

public class OrderApprovedMessage(OrderApproved payload) : JsonMessage<OrderApproved>(payload);
