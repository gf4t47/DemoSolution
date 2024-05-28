## Command and Handler
### Plan Delivery Command

1. Triggered by [Delivery Scheduled](###delivery-scheduled-message) message.
2. Mock method just print info for now.
### Make Delivery Command

1. Triggered by [Dishes Ready](###deshes-ready-message) message.
2. Send queue message [Delivery Completed](###delivery-completed-message) to [Head Quarter](./Main-Component%20Head-Quarter.md).
3. Update order status in persistence layer.
## Receive Message
### Delivery Scheduled Message

1. Received from [Head Quarter](./Main-Component%20Head-Quarter.md)
2. `CommandBus.Execute(`[`PlanDeliveryCommand`](###plan-delivery-command)`)`

### Dishes Ready Message

1. Received from [Work Shop](./Sub-Component%20Workshop.md)
2. `CommandBus.Execute(`[`MakeDeliveryCommand`](###make-delivery-command)`)`
## Send Message
### Delivery Completed Message

Send to [Head Quarter](./Main-Component%20Head-Quarter.md)
