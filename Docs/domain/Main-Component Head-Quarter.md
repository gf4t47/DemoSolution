## Command and Handler
### Deliver Dishes Command

1. Command triggered by [Order Approved](###order-approved-message) message.
2. Send queue message [`DeliveryScheduled`](###delivery-scheduled-message) to [Delivery Component](./Sub-Component%20Delivery)
### Make Dishes Command

1. Command triggered by [Order Approved](###order-approved-message) message.
2. Update order status in persistence layer.
3. Send queue message [`DishesScheduled`](###dishes-scheduled-message) to [Workshop Component](./Sub-Component%20Workshop).

### Order Completed Command

1. Update order status in persistence layer.
2. Print message that one order is completed.
## Receive Message
### Order Approved Message

1. Received from [Ordering Component](./Sub-Component%20Ordering.md).
2. `CommandBus.Execute(`[`MakeDishesCommand`](###deliver-dishes-command)`)`
3. `CommandBus.Execute(`[`DeliverDishesCommand`](###deliver-dishes-command)`)`
### Delivery Completed Message

1. Received from [Delivery Component](./Sub-Component%20Delivery)
2. `CommandBus.Execute(`[`OrderCompletedCommand`](###order-completed-command)`)`
## Send Message
### Dishes Scheduled Message

Send to  [Delivery Component](./Sub-Component%20Delivery)
### Delivery Scheduled Message

Send to [Workshop Component](./Sub-Component%20Workshop).
