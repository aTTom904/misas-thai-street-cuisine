using System.Text.Json;

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
            _logger.LogInformation($"Starting CreateOrderAsync - API Base URL: {_apiBaseUrl}");
            _logger.LogInformation($"Request Data - Customer: {request.CustomerName}, Email: {request.CustomerEmail}, Total: {request.Total:C}");
            _logger.LogInformation($"Payment Token: {(!string.IsNullOrEmpty(request.PaymentToken) ? "Present" : "Missing")}");
            _logger.LogInformation($"Items Count: {request.Items?.Count ?? 0}");

            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            _logger.LogInformation($"Serialized JSON (first 500 chars): {json[..Math.Min(json.Length, 500)]}");

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            _logger.LogInformation($"Making POST request to: {_apiBaseUrl}/TakePayment");
            _logger.LogInformation($"HttpClient BaseAddress: {_httpClient.BaseAddress}");
            _logger.LogInformation($"Content-Type: application/json, Encoding: UTF8");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/TakePayment", content);
            
            _logger.LogInformation($"Response Status: {response.StatusCode} ({(int)response.StatusCode})");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("API call successful - reading response content");
                var responseJson = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response JSON: {responseJson}");
                
                var orderResponse = JsonSerializer.Deserialize<OrderResponse>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation($"Deserialized response - OrderNumber: {orderResponse?.OrderNumber}, Status: {orderResponse?.Status}");

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
                _logger.LogError($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                
                return new ApiResponse<OrderResponse>
                {
                    Success = false,
                    Error = $"API Error: {response.StatusCode} - {errorContent}"
                };
            }
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP Request Exception occurred");
            _logger.LogError($"HTTP Exception Message: {httpEx.Message}");
            _logger.LogError($"HTTP Exception Data: {httpEx.Data}");
            return new ApiResponse<OrderResponse>
            {
                Success = false,
                Error = $"HTTP Request error: {httpEx.Message}"
            };
        }
        catch (TaskCanceledException tcEx)
        {
            _logger.LogError(tcEx, "Request timed out");
            return new ApiResponse<OrderResponse>
            {
                Success = false,
                Error = $"Request timeout: {tcEx.Message}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling order API");
            _logger.LogError($"Exception Type: {ex.GetType().Name}");
            _logger.LogError($"Exception Message: {ex.Message}");
            _logger.LogError($"Stack Trace: {ex.StackTrace}");
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