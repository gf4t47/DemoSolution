## Command and Handler
### Cooking Dishes Command

1. Command Triggered By [Dishes Scheduled](###dishes-scheduled-message) 
2. Send queue message [Dishes Ready](###dishes-ready-message) to [Delivery Component](./Sub-Component%20Delivery.md)
3. Update order status in persistence layer.
## Receive Message

### Dishes Scheduled Message

1. Received from [Head Quarter Component](./Main-Component%20Head-Quarter). 
2. `CommandBus.Execute(`[`Cooking`](###cooking-dishes-command)`)`.
## Send Message

### Dishes Ready Message

Send to [Delivery Component](./Sub-Component%20Delivery.md)
