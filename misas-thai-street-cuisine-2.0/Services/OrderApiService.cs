using System.Text.Json;

namespace misas_thai_street_cuisine_2._0.Services;

public class OrderApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderApiService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ApiConfigurationService _apiConfigService;
    private string _apiBaseUrl = string.Empty;

    public OrderApiService(HttpClient httpClient, ILogger<OrderApiService> logger, IConfiguration configuration, ApiConfigurationService apiConfigService)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        _apiConfigService = apiConfigService;
    }

    private void EnsureApiBaseUrlLoaded()
    {
        if (string.IsNullOrEmpty(_apiBaseUrl))
        {
            try
            {
                // Try to get from API config first
                _apiBaseUrl = _apiConfigService.GetApiBaseUrl();
            }
            catch (Exception)
            {
                // Fall back to static configuration
                _apiBaseUrl = _configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("API Base URL not configured");
            }
        }
    }

    public async Task<ApiResponse<OrderResponse>> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            EnsureApiBaseUrlLoaded();
            
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
            EnsureApiBaseUrlLoaded();
            
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
    public string AdditionalInformation { get; set; } = string.Empty;
    public bool ConsentToUpdates { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string DeliveryDate { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string PaymentToken { get; set; } = string.Empty;
    public List<OrderItemRequest> Items { get; set; } = new();
    public decimal TipAmount { get; set; }
    public decimal SalesTax { get; set; }
}

public class OrderItemRequest
{
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int? SelectedServes { get; set; }
    public int UpgradePhadThai24Qty { get; set; }
    public int UpgradePhadThai48Qty { get; set; }
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