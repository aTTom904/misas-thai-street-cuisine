using Microsoft.JSInterop;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class DeliveryValidationService
    {
        private readonly IJSRuntime _jsRuntime;

        public DeliveryValidationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Shared validation state
        public string? LastValidatedAddress { get; private set; }
        public DeliveryValidationResult? LastValidationResult { get; private set; }
        public bool HasValidatedAddress => !string.IsNullOrEmpty(LastValidatedAddress) && LastValidationResult != null;

        // Constants from DeliveryArea component
        private const string RouteStartAddress = "1301 N Orange Ave Suite 102, Green Cove Springs, FL 32043";
        private const string RouteEndAddress = "850 Beacon Lake Pkwy, St. Augustine, FL 32095";
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
                var userLocation = await _jsRuntime.InvokeAsync<LatLngResult>("geocodeAddress", address);
                if (userLocation == null)
                {
                    result = new DeliveryValidationResult
                    {
                        IsInDeliveryArea = false,
                        Message = "Could not find your address. Please check your input.",
                        ResultType = "error"
                    };
                }
                else
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
    }
}