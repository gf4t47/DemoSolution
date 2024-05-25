// Copyright (c) Demo.
namespace Ordering.Message;

using Communication;

public record OrderApproved();

public class OrderApprovedMessage(OrderApproved payload) : JsonMessage<OrderApproved>(payload);
