The use case will be an Online food ordering company.

## Technique Stack

SDK: .NET Framework 4.8.1
```XML
<TargetFrameworkVersion>v4.8.1</TargetFrameworkVersion>
```

Language: C# 7.3 (default version of .NET framework 4.8.1)
```XML
<LangVersion>default</LangVersion>
```

 Nullable references and static analysis: On
``` XML
<Nullable>enable</Nullable>
```

## Dependent Packages

`Microsoft.Extensions.DependencyInjection`: Dependency Injection solution
`System.Text.Json`: message serialization

## Use Case
### [Head Quarter (Central Component)](Main-Component%20Head-Quarter.md)

- Central coordinator in an Orchestration architecture.
- Queue message forwarder.

### [Ordering (accept order)](Sub-Component%20Ordering.md)

- Public facing Service, Taking Order from customer (ideally via Restful API)
	- Create `Order` entity in persistence layer.
- In charge of payment
	- Based on payment approved or not, update `Order` entity in persistence layer.
- If payment approved, send message to **Head Quarter** for next stage.

### [Workshop (cook the food)](Sub-Component%20Workshop.md)

- Kitchen facing Service.
- Take dishes to cook via queue message from **Head Quarter**
- Update `Order` entity in persistence layer.

### [Delivery (delivery the food)](Sub-Component%20Delivery.md)

- Rider facing Service.
- Take `Workshop`  location via queue message from **Head Quarter** as source.
- Take `Delivery Address` location via queue message from **Head Quarter** as destination.
- Update `Order` entity status in persistence layer.