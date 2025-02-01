# DomainCraft

**DomainCraft** is a modular and extensible framework designed to simplify the implementation of **Clean Architecture** and **Domain-Driven Design (DDD)** in .NET applications. It provides ready-to-use utilities like **Generic Repositories, Caching, Logging, Event Bus, Dependency Injection Helpers, and more**.

## 📦 Installation

To install DomainCraft, use NuGet Package Manager:

```sh
Install-Package DomainCraft
```

Or via .NET CLI:

```sh
dotnet add package DomainCraft
```

## 🚀 Features

### 1️⃣ **EF Core Generic Repository & Unit of Work**

Provides a **generic repository pattern** with built-in Unit of Work.

```csharp
services.AddDomainCraftEFCore();
```

✅ **Includes:**

- Generic Repository (`IRepository<T, TKey>`)
- Unit of Work (`IUnitOfWork`)
- Query Support (`IQueryable`, `AsNoTracking`)

---

### 2️⃣ **Caching with Redis**

Provides **distributed caching** with Redis.

```csharp
services.AddDomainCraftCaching(options =>
{
    options.RedisConnectionString = "localhost:6379";
    options.InstanceName = "MyAppCache";
});
```

✅ **Features:**

- Supports **in-memory** and **Redis**
- Configurable expiration & eviction policies

---

### 3️⃣ **Logging Middleware (Serilog + Elasticsearch)**

Automatically logs API requests & responses and sends them to **Elasticsearch**.

```csharp
services.AddDomainCraftLogging(configuration);
```

✅ **Features:**

- **Correlation ID** support for tracing requests
- Structured logging using **Serilog**
- Integration with **Elasticsearch**

---

### 4️⃣ **Event Bus (MassTransit + RabbitMQ)**

Implements **Event-Driven Architecture** with **MassTransit** and **RabbitMQ**.

```csharp
services.AddDomainCraftEventBus(options =>
{
    options.Host = "rabbitmq://localhost";
    options.Username = "guest";
    options.Password = "guest";
});
```

✅ **Features:**

- **Publish/Subscribe** mechanism for microservices
- **Retry Policies** and **Circuit Breaker** with **Polly**
- Supports **Multiple Consumers**

**📌 Publishing an Event:**

```csharp
await _eventBus.PublishAsync(new OrderCreatedEvent(orderId));
```

**📌 Consuming an Event:**

```csharp
public class OrderCreatedConsumer : EventConsumer<OrderCreatedEvent>
{
    public override async Task ConsumeAsync(OrderCreatedEvent message)
    {
        Console.WriteLine($"Processing order: {message.OrderId}");
    }
}
```

---

## 🛠 **Registering DomainCraft in Your Application**

In **Program.cs**, register **DomainCraft**:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomainCraft(builder.Configuration, options =>
{
    options.UseCaching = true;
    options.ConfigureCache = cacheOptions =>
    {
        cacheOptions.RedisConnectionString = "localhost:6379";
    };
});

var app = builder.Build();
app.UseDomainCraftMiddlewares();
app.Run();
```

---

## 🔥 **Contributing**

We welcome contributions! Feel free to open an **Issue** or **Pull Request**.

## ⭐ **License**

DomainCraft is open-source under the **MIT License**.

🚀 **Happy Coding!**

