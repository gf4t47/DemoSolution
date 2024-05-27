## Command and Handler
### Submit Order Command

1. Command triggered by customer, ideally prepare command data from Restful API request.
2. Calculate order price
3. Call payment API to pay the price
	1. payment succeed: `CommandBus.Execute(`[`AcceptOrderCommand`](###accept-order-command)`)`
	2. payment failed: `CommandBus.Execute(`[`RejectOrderCommand`](###reject-order-command)`)`

### Cancel Order Command (Not implemented)

1. Command triggered by customer, cancel the order they submitted.
### Accept Order Command

1. Triggered by [`SubmitOrderHandler`](###submit-order-command), in case of payment succeed.
2. Send queue message [`OrderApproved`](###order-approved-message) to [Head Quarter Component](./Main-Component%20Head-Quarter.md)

### Reject Order Command

1. Triggered by [`SubmitOrderHandler`](###submit-order-command), in case of payment failed.
2. Notify client (browser, app, etc.) order failed (not implement).

## Send Message
### Order Approved Message

Send to [Head Quarter](./Main-Component%20Head-Quarter.md)