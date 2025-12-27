using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using misas_thai_street_cuisine_2._0.EmailTemplates;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class GoogleMapsConfigService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly ApiConfigurationService _apiConfigService;
        private readonly ILogger<GoogleMapsConfigService> _logger;
        private readonly EmailService _emailService;
        private string _apiKey = string.Empty;
        private bool _isLoaded = false;

        public GoogleMapsConfigService(
            IJSRuntime jsRuntime, 
            IConfiguration configuration, 
            ApiConfigurationService apiConfigService,
            ILogger<GoogleMapsConfigService> logger,
            EmailService emailService)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _apiConfigService = apiConfigService;
            _logger = logger;
            _emailService = emailService;
        }

        private void EnsureApiKeyLoaded()
        {
            if (!string.IsNullOrEmpty(_apiKey)) return;

            // Try API config first
            try
            {
                _apiKey = _apiConfigService.GetGoogleMapsApiKey();
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    _logger.LogInformation("Google Maps API Key loaded from API configuration");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load API Key from API, falling back to static config");
            }

            // Fall back to static configuration
            _apiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;
            
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("Google Maps API Key not configured in any source");
            }

            _logger.LogInformation("Google Maps API Key loaded from static configuration");
        }

        public async Task LoadGoogleMapsAsync()
        {
            if (_isLoaded) return;
            
            EnsureApiKeyLoaded();

            // Try primary method
            try
            {
                _logger.LogInformation("Loading Google Maps with loadGoogleMapsScript");
                await _jsRuntime.InvokeVoidAsync("loadGoogleMapsScript", _apiKey);
                _isLoaded = true;
                _logger.LogInformation("Google Maps loaded successfully");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Primary load method failed, trying fallback");
            }

            // Try fallback method
            try
            {
                await _jsRuntime.InvokeVoidAsync("initializeGoogleMaps", _apiKey);
                _isLoaded = true;
                _logger.LogInformation("Google Maps loaded successfully with fallback method");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "All Google Maps loading methods failed");
                
                // Send error notification email
                await SendErrorNotificationAsync(
                    "GoogleMapsConfigService.LoadGoogleMapsAsync",
                    "Google Maps JavaScript API",
                    ex,
                    "Both primary and fallback methods failed to load Google Maps script"
                );
                
                throw new InvalidOperationException("Failed to load Google Maps script. Check browser console for details.", ex);
            }
        }

        public bool IsLoaded => _isLoaded;
        public string ApiKey 
        { 
            get 
            { 
                EnsureApiKeyLoaded(); 
                return _apiKey; 
            } 
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
}