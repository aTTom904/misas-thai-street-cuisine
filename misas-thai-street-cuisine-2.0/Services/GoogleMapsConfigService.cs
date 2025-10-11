using Microsoft.JSInterop;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class GoogleMapsConfigService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private bool _isLoaded = false;

        public GoogleMapsConfigService(IJSRuntime jsRuntime, IConfiguration configuration)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _apiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;
            
            Console.WriteLine($"GoogleMapsConfigService initialized with API Key: {(!string.IsNullOrEmpty(_apiKey) ? "CONFIGURED" : "NOT CONFIGURED")}");
            
            if (string.IsNullOrEmpty(_apiKey))
            {
                Console.WriteLine("Warning: Google Maps API Key not configured. Maps functionality will be disabled.");
                Console.WriteLine("Available configuration keys:");
                foreach (var config in _configuration.AsEnumerable())
                {
                    Console.WriteLine($"  {config.Key} = {config.Value}");
                }
            }
        }

        public async Task LoadGoogleMapsAsync()
        {
            if (_isLoaded) return;
            
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("Google Maps API Key not configured");
            }

            try
            {
                Console.WriteLine("Attempting to load Google Maps with loadGoogleMapsScript...");
                
                // Try the original method first
                try
                {
                    await _jsRuntime.InvokeVoidAsync("loadGoogleMapsScript", _apiKey);
                    _isLoaded = true;
                    Console.WriteLine("Google Maps loaded successfully with loadGoogleMapsScript");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"loadGoogleMapsScript failed: {ex.Message}, trying fallback...");
                }

                // Fallback method
                await _jsRuntime.InvokeVoidAsync("initializeGoogleMaps", _apiKey);
                _isLoaded = true;
                Console.WriteLine("Google Maps loaded successfully with fallback method");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"All Google Maps loading methods failed: {ex.Message}");
                throw new InvalidOperationException("Failed to load Google Maps script", ex);
            }
        }

        public bool IsLoaded => _isLoaded;
        public string ApiKey => _apiKey;
    }
}