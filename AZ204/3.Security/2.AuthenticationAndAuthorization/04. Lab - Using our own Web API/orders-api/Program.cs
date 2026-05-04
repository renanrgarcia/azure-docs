using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

var orders = new ConcurrentDictionary<int, Order>();

// Seed a few orders
orders.TryAdd(1001, new Order(1001, "Customer 001", 49.00m, "Created"));
orders.TryAdd(1002, new Order(1002, "Customer 002", 99.00m, "Paid"));
orders.TryAdd(1003, new Order(1003, "Customer 003", 149.00m, "Shipped"));

app.MapGet("/api/orders", () =>
{
    return Results.Ok(orders.Values.OrderBy(o => o.OrderId));
})
.WithName("GetOrders");

app.MapGet("/api/orders/{id:int}", (int id) =>
{
    return orders.TryGetValue(id, out var order)
        ? Results.Ok(order)
        : Results.NotFound(new { message = $"Order {id} not found." });
})
.WithName("GetOrderById");

app.MapPost("/api/orders", (CreateOrderRequest request) =>
{
    if (request.OrderId <= 0)
        return Results.BadRequest(new { message = "OrderId must be greater than 0." });

    var newOrder = new Order(
        request.OrderId,
        request.CustomerName.Trim(),
        request.Amount,
        "Created"
    );

    if (!orders.TryAdd(newOrder.OrderId, newOrder))
        return Results.Conflict(new { message = $"Order {newOrder.OrderId} already exists." });

    return Results.Created($"/api/orders/{newOrder.OrderId}", newOrder);
})
.WithName("CreateOrder");


app.Run();

record Order(int OrderId, string CustomerName, decimal Amount, string Status);

record CreateOrderRequest(int OrderId, string CustomerName, decimal Amount);
