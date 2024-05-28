The use case will be an Online food ordering company.
## Demo

[`HeadQuarter.Console`](../Src/HeadQuarter.Console/Program.cs) is the entry point console executable for demo purpose.
## Tech Stack

SDK: .NET Framework 4.8.1
```XML
<TargetFrameworkVersion>v4.8.1</TargetFrameworkVersion>
```

Language: C# Latest Minor Version
```XML
<LangVersion>default</LangVersion>
```

 Nullable references and static analysis: On
``` XML
<Nullable>enable</Nullable>
```

## Dependent Packages

- `Microsoft.Extensions.DependencyInjection`: Dependency Injection solution
- `System.Text.Json`: message serialization

## Use Case

To simplify the use case to achieve MVP asap, here are some constrains:
1. immediate order only, no schedule support yet to NOT introduce the complexity of time.
2. Cancel a submitted order is NOT implemented.
### [Head Quarter (Central Component)](./domain/Main-Component%20Head-Quarter.md)

- Central coordinator in an Orchestration architecture.
- Queue message forwarder.

### [Ordering (accept order)](./domain/Sub-Component%20Ordering.md)

- Public facing Service, Taking Order from customer (ideally via Restful API)
	- Create `Order` entity in persistence layer.
- In charge of payment
	- Based on payment approved or not, update `Order` entity in persistence layer.
- If payment approved, send message to **Head Quarter** for next stage.

### [Workshop (cook the food)](./domain/Sub-Component%20Workshop.md)

- Kitchen facing Service.
- Take dishes to cook via queue message from **Head Quarter**
- Update `Order` entity in persistence layer.

### [Delivery (delivery the food)](./domain/Sub-Component%20Delivery.md)

- Rider facing Service.
- Take `Workshop`  location via queue message from **Head Quarter** as source.
- Take `Delivery Address` location via queue message from **Head Quarter** as destination.
- Update `Order` entity status in persistence layer.

## System Overview

### Inside a component

Inside one component, use [`command` <=> `command handler` pattern](./core/Command%20Handler%20Pattern.md) to achieve main logic flow.
### Cross components

Cross components, rely on queue message as main [Communication](./infrastructure/Communication.md) methodology.