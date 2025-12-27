using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using misas_thai_street_cuisine_2._0.EmailTemplates;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class SquarePaymentService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly ApiConfigurationService _apiConfigService;
        private readonly ILogger<SquarePaymentService> _logger;
        private readonly EmailService _emailService;
        private string _applicationId = string.Empty;
        private string _locationId = string.Empty;

        public SquarePaymentService(
            IJSRuntime jsRuntime, 
            IConfiguration configuration, 
            ApiConfigurationService apiConfigService,
            ILogger<SquarePaymentService> logger,
            EmailService emailService)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _apiConfigService = apiConfigService;
            _logger = logger;
            _emailService = emailService;
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
            try
            {
                EnsureConfigurationLoaded();
                var result = await _jsRuntime.InvokeAsync<PaymentInitResult>(
                    "squarePayments.init", _applicationId, _locationId);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Square payment initialization failed: {Error}", result.Error);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during Square payment initialization");
                
                await SendErrorNotificationAsync(
                    "SquarePaymentService.InitializeAsync",
                    "Square Payments API",
                    ex,
                    $"Failed to initialize Square payments with Application ID: {_applicationId}"
                );
                
                return new PaymentInitResult { Success = false, Error = ex.Message };
            }
        }

        public async Task<PaymentInitResult> InitializeCardAsync(string containerId)
        {
            var result = await _jsRuntime.InvokeAsync<PaymentInitResult>(
                "squarePayments.initCard", containerId);
            return result;
        }

        public async Task<TokenizeResult> TokenizeCardAsync()
        {
            try
            {
                var result = await _jsRuntime.InvokeAsync<TokenizeResult>(
                    "squarePayments.tokenizeCard");
                
                if (!result.Success && result.Errors != null && result.Errors.Length > 0)
                {
                    _logger.LogWarning("Square card tokenization failed: {Errors}", string.Join(", ", result.Errors));
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during Square card tokenization");
                
                await SendErrorNotificationAsync(
                    "SquarePaymentService.TokenizeCardAsync",
                    "Square Payments API",
                    ex,
                    "Failed to tokenize credit card"
                );
                
                return new TokenizeResult { Success = false, Error = ex.Message };
            }
        }

        public async Task<bool> IsApplePayAvailableAsync()
        {
            try
            {
                EnsureConfigurationLoaded();
                var result = await _jsRuntime.InvokeAsync<bool>(
                    "squarePayments.isApplePayAvailable");
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PaymentInitResult> InitializeApplePayAsync(string containerId, decimal amount, string label = "Total")
        {
            var result = await _jsRuntime.InvokeAsync<PaymentInitResult>(
                "squarePayments.initApplePay", containerId, amount, label);
            return result;
        }

        public async Task<TokenizeResult> TokenizeApplePayAsync()
        {
            var result = await _jsRuntime.InvokeAsync<TokenizeResult>(
                "squarePayments.tokenizeApplePay");
            return result;
        }

        public async Task DestroyAsync()
        {
            await _jsRuntime.InvokeVoidAsync("squarePayments.destroy");
        }

        private async Task SendErrorNotificationAsync(string location, string integration, Exception ex, string? additionalInfo = null)
        {
            try
            {
                var adminEmail = _configuration["AdminEmail"];
                if (string.IsNullOrEmpty(adminEmail)) return;

                var errorData = new ErrorNotificationData(
                    Location: location,
                    IntegrationName: integration,
                    ErrorType: ex.GetType().Name,
                    ErrorMessage: ex.Message,
                    Timestamp: DateTime.UtcNow,
                    StackTrace: ex.StackTrace,
                    AdditionalInfo: additionalInfo
                );

                var emailContent = ErrorNotificationEmail.Create(errorData);
                await _emailService.SendEmailAsync(adminEmail, emailContent);
            }
            catch
            {
                // Silently fail - don't let email errors break the service
            }
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