using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using misas_thai_street_cuisine_2._0.EmailTemplates;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class DeliveryValidationService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly GoogleMapsConfigService _googleMapsConfig;
        private readonly ILogger<DeliveryValidationService> _logger;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public DeliveryValidationService(
            IJSRuntime jsRuntime, 
            GoogleMapsConfigService googleMapsConfig,
            ILogger<DeliveryValidationService> logger,
            EmailService emailService,
            IConfiguration configuration)
        {
            _jsRuntime = jsRuntime;
            _googleMapsConfig = googleMapsConfig;
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
        }

        // Shared validation state
        public string? LastValidatedAddress { get; private set; }
        public DeliveryValidationResult? LastValidationResult { get; private set; }
        public bool HasValidatedAddress => !string.IsNullOrEmpty(LastValidatedAddress) && LastValidationResult != null;

        // Constants from DeliveryArea component
        private const string RouteStartAddress = "1301 N Orange Ave Suite 102, Green Cove Springs, FL 32043";
        private const string RouteEndAddress = "211 Davis Park Rd, Ponte Vedra Beach, FL 32081";
        private const double BufferMiles = 5.0;
        private const bool IsFreeDeliveryForEveryone = false;
        private const string FreeDeliveryMessage = "🎉 Special Offer: We're currently offering FREE delivery for everyone! No minimum distance required.";
        private const string OutOfRangeMessage = "We'd love to deliver to you soon! Your area isn't in our delivery range yet, but we're expanding. Follow us on social media for updates";

        public class LatLngResult
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class DeliveryValidationResult
        {
            public bool IsInDeliveryArea { get; set; }
            public string Message { get; set; } = string.Empty;
            public string ResultType { get; set; } = string.Empty; // "success", "warning", "error"
        }

        public async Task<DeliveryValidationResult> ValidateDeliveryAddress(string address)
        {
            DeliveryValidationResult result;

            // Check for free delivery override first
            if (IsFreeDeliveryForEveryone)
            {
                result = new DeliveryValidationResult
                {
                    IsInDeliveryArea = true,
                    Message = FreeDeliveryMessage,
                    ResultType = "success"
                };
            }
            else
            {
                try
                {
                    // Ensure Google Maps is loaded before making API calls
                    await _googleMapsConfig.LoadGoogleMapsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load Google Maps for address validation");
                    
                    await SendErrorNotificationAsync(
                        "DeliveryValidationService.ValidateDeliveryAddress",
                        "Google Maps API",
                        ex,
                        $"Failed to load Google Maps when validating address: {address}"
                    );
                    
                    return new DeliveryValidationResult
                    {
                        IsInDeliveryArea = false,
                        Message = "Unable to validate delivery address. Please try again later.",
                        ResultType = "error"
                    };
                }

                var userLocation = await _jsRuntime.InvokeAsync<LatLngResult>("geocodeAddress", address);
                if (userLocation == null)
                {
                    _logger.LogWarning("Failed to geocode address: {Address}", address);
                    
                    result = new DeliveryValidationResult
                    {
                        IsInDeliveryArea = false,
                        Message = "Could not find your address. Please check your input.",
                        ResultType = "error"
                    };
                }
                else
                {
                    try
                    {
                        // Check if user is within buffer of the static route
                        var isNearRoute = await _jsRuntime.InvokeAsync<bool>("isPointNearRoute",
                            RouteStartAddress, RouteEndAddress, userLocation.lat, userLocation.lng, BufferMiles);

                        if (isNearRoute)
                        {
                            result = new DeliveryValidationResult
                            {
                                IsInDeliveryArea = true,
                                Message = "Great news! You're within our free delivery corridor.",
                                ResultType = "success"
                            };
                        }
                        else
                        {
                            result = new DeliveryValidationResult
                            {
                                IsInDeliveryArea = false,
                                Message = OutOfRangeMessage,
                                ResultType = "warning"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error calculating route distance for address: {Address}", address);
                        
                        await SendErrorNotificationAsync(
                            "DeliveryValidationService.ValidateDeliveryAddress",
                            "Google Maps Directions API",
                            ex,
                            $"Failed to calculate route for address: {address} (Coordinates: {userLocation.lat}, {userLocation.lng})"
                        );
                        
                        result = new DeliveryValidationResult
                        {
                            IsInDeliveryArea = false,
                            Message = "Unable to validate delivery area. Please try again later.",
                            ResultType = "error"
                        };
                    }
                }
            }

            // Store the validation result for cross-page sharing
            LastValidatedAddress = address;
            LastValidationResult = result;

            return result;
        }

        public bool IsAddressAlreadyValidated(string address)
        {
            return HasValidatedAddress && 
                   string.Equals(LastValidatedAddress?.Trim(), address.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public void ClearValidation()
        {
            LastValidatedAddress = null;
            LastValidationResult = null;
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