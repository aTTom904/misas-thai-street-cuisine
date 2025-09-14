using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MisasThaiApi.Models;
using Microsoft.Data.SqlClient;

namespace MisasThaiApi.Functions;

public class OrderFunctions
{
    private readonly ILogger _logger;
    private readonly string _connectionString;

    public OrderFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<OrderFunctions>();
        _connectionString = Environment.GetEnvironmentVariable("SqlConnectionString") ?? "";
    }

    [Function("CreateOrder")]
    public async Task<HttpResponseData> CreateOrder(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")] HttpRequestData req)
    {
        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var orderRequest = JsonSerializer.Deserialize<CreateOrderRequest>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (orderRequest == null)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid request body");
                return badResponse;
            }

            // Generate order number
            var orderNumber = $"ORDER-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";

            // Create order in database
            var orderId = await SaveOrderToDatabase(orderRequest, orderNumber);

            var response = req.CreateResponse(HttpStatusCode.Created);
            var orderResponse = new
            {
                OrderId = orderId,
                OrderNumber = orderNumber,
                Status = "Created",
                Message = "Order created successfully"
            };

            await response.WriteStringAsync(JsonSerializer.Serialize(orderResponse));
            response.Headers.Add("Content-Type", "application/json");

            _logger.LogInformation($"Order created: {orderNumber}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync($"Error creating order: {ex.Message}");
            return errorResponse;
        }
    }

    [Function("GetOrders")]
    public async Task<HttpResponseData> GetOrders(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders")] HttpRequestData req)
    {
        try
        {
            var orders = await GetOrdersFromDatabase();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(JsonSerializer.Serialize(orders));
            response.Headers.Add("Content-Type", "application/json");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync($"Error retrieving orders: {ex.Message}");
            return errorResponse;
        }
    }

    private async Task<int> SaveOrderToDatabase(CreateOrderRequest orderRequest, string orderNumber)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();
        try
        {
            // Insert order
            var orderQuery = @"
                INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, 
                                  ConsentToUpdates, Total, OrderDate, PaymentToken, Status)
                OUTPUT INSERTED.Id
                VALUES (@OrderNumber, @CustomerName, @CustomerEmail, @CustomerPhone, 
                       @ConsentToUpdates, @Total, @OrderDate, @PaymentToken, @Status)";

            var orderCommand = new SqlCommand(orderQuery, connection, transaction);
            orderCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
            orderCommand.Parameters.AddWithValue("@CustomerName", orderRequest.CustomerName);
            orderCommand.Parameters.AddWithValue("@CustomerEmail", orderRequest.CustomerEmail);
            orderCommand.Parameters.AddWithValue("@CustomerPhone", orderRequest.CustomerPhone ?? "");
            orderCommand.Parameters.AddWithValue("@ConsentToUpdates", orderRequest.ConsentToUpdates);
            orderCommand.Parameters.AddWithValue("@Total", orderRequest.Total);
            orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.UtcNow);
            orderCommand.Parameters.AddWithValue("@PaymentToken", orderRequest.PaymentToken);
            orderCommand.Parameters.AddWithValue("@Status", "Completed");

            var orderId = (int)await orderCommand.ExecuteScalarAsync();

            // Insert order items
            foreach (var item in orderRequest.Items)
            {
                var itemQuery = @"
                    INSERT INTO MenuItems (OrderId, ItemName, Category, Price, Quantity)
                    VALUES (@OrderId, @ItemName, @Category, @Price, @Quantity)";

                var itemCommand = new SqlCommand(itemQuery, connection, transaction);
                itemCommand.Parameters.AddWithValue("@OrderId", orderId);
                itemCommand.Parameters.AddWithValue("@ItemName", item.ItemName);
                itemCommand.Parameters.AddWithValue("@Category", item.Category);
                itemCommand.Parameters.AddWithValue("@Price", item.Price);
                itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);

                await itemCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return orderId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<List<Order>> GetOrdersFromDatabase()
    {
        var orders = new List<Order>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            SELECT o.*, mi.ItemName, mi.Category, mi.Price, mi.Quantity
            FROM Orders o
            LEFT JOIN MenuItems mi ON o.Id = mi.OrderId
            ORDER BY o.OrderDate DESC";

        var command = new SqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        var orderDict = new Dictionary<int, Order>();

        while (await reader.ReadAsync())
        {
            var orderId = reader.GetInt32("Id");

            if (!orderDict.ContainsKey(orderId))
            {
                orderDict[orderId] = new Order
                {
                    Id = orderId,
                    OrderNumber = reader.GetString("OrderNumber"),
                    CustomerName = reader.GetString("CustomerName"),
                    CustomerEmail = reader.GetString("CustomerEmail"),
                    CustomerPhone = reader.GetString("CustomerPhone"),
                    ConsentToUpdates = reader.GetBoolean("ConsentToUpdates"),
                    Total = reader.GetDecimal("Total"),
                    OrderDate = reader.GetDateTime("OrderDate"),
                    PaymentToken = reader.GetString("PaymentToken"),
                    Status = reader.GetString("Status"),
                    Items = new List<OrderItem>()
                };
            }

            if (!reader.IsDBNull("ItemName"))
            {
                orderDict[orderId].Items.Add(new OrderItem
                {
                    ItemName = reader.GetString("ItemName"),
                    Category = reader.GetString("Category"),
                    Price = reader.GetDecimal("Price"),
                    Quantity = reader.GetInt32("Quantity")
                });
            }
        }

        return orderDict.Values.ToList();
    }
}