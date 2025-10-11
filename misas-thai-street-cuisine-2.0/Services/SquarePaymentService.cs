using Microsoft.JSInterop;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class SquarePaymentService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly ApiConfigurationService _apiConfigService;
        private string _applicationId = string.Empty;
        private string _locationId = string.Empty;

        public SquarePaymentService(IJSRuntime jsRuntime, IConfiguration configuration, ApiConfigurationService apiConfigService)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _apiConfigService = apiConfigService;
        }

        private void EnsureConfigurationLoaded()
        {
            if (string.IsNullOrEmpty(_applicationId) || string.IsNullOrEmpty(_locationId))
            {
                try
                {
                    // Try to get from API config first
                    _applicationId = _apiConfigService.GetSquareApplicationId();
                    _locationId = _apiConfigService.GetSquareLocationId();
                }
                catch (Exception)
                {
                    // Fall back to static configuration
                    _applicationId = _configuration["Square:ApplicationId"] ?? throw new InvalidOperationException("Square Application ID not configured");
                    _locationId = _configuration["Square:LocationId"] ?? throw new InvalidOperationException("Square Location ID not configured");
                }
            }
        }

        public async Task<PaymentInitResult> InitializeAsync()
        {
            EnsureConfigurationLoaded();
            var result = await _jsRuntime.InvokeAsync<PaymentInitResult>(
                "squarePayments.init", _applicationId, _locationId);
            return result;
        }

        public async Task<PaymentInitResult> InitializeCardAsync(string containerId)
        {
            var result = await _jsRuntime.InvokeAsync<PaymentInitResult>(
                "squarePayments.initCard", containerId);
            return result;
        }

        public async Task<TokenizeResult> TokenizeCardAsync()
        {
            var result = await _jsRuntime.InvokeAsync<TokenizeResult>(
                "squarePayments.tokenizeCard");
            return result;
        }

        public async Task DestroyAsync()
        {
            await _jsRuntime.InvokeVoidAsync("squarePayments.destroy");
        }
    }

    public class PaymentInitResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class TokenizeResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public object? Details { get; set; }
        public object[]? Errors { get; set; }
        public string? Error { get; set; }
    }
}