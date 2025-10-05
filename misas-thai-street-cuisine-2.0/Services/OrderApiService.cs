using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace misas_thai_street_cuisine_2._0.Services;

public class OrderApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderApiService> _logger;
    private readonly string _apiBaseUrl;

    public OrderApiService(HttpClient httpClient, ILogger<OrderApiService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiBaseUrl = configuration["ApiBaseUrl"] ?? "https://misas-thai-api.azurewebsites.net/api";
    }

    public async Task<ApiResponse<OrderResponse>> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/TakePayment", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var orderResponse = JsonSerializer.Deserialize<OrderResponse>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new ApiResponse<OrderResponse>
                {
                    Success = true,
                    Data = orderResponse
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API Error: {response.StatusCode} - {errorContent}");
                
                return new ApiResponse<OrderResponse>
                {
                    Success = false,
                    Error = $"API Error: {response.StatusCode} - {errorContent}"
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling order API");
            return new ApiResponse<OrderResponse>
            {
                Success = false,
                Error = $"Network error: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<List<OrderSummary>>> GetOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/orders");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var orders = JsonSerializer.Deserialize<List<OrderSummary>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new ApiResponse<List<OrderSummary>>
                {
                    Success = true,
                    Data = orders ?? new List<OrderSummary>()
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<List<OrderSummary>>
                {
                    Success = false,
                    Error = $"API Error: {response.StatusCode} - {errorContent}"
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders");
            return new ApiResponse<List<OrderSummary>>
            {
                Success = false,
                Error = $"Network error: {ex.Message}"
            };
        }
    }
}

// API Models
public class CreateOrderRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public bool ConsentToUpdates { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string DeliveryDate { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string PaymentToken { get; set; } = string.Empty;
    public List<OrderItemRequest> Items { get; set; } = new();
}

public class OrderItemRequest
{
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class OrderResponse
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class OrderSummary
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public string ItemsSummary { get; set; } = string.Empty;
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Error { get; set; } = string.Empty;
}