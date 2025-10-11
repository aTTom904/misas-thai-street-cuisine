using Microsoft.JSInterop;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class GoogleMapsConfigService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly ApiConfigurationService _apiConfigService;
        private string _apiKey = string.Empty;
        private bool _isLoaded = false;

        public GoogleMapsConfigService(IJSRuntime jsRuntime, IConfiguration configuration, ApiConfigurationService apiConfigService)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _apiConfigService = apiConfigService;
        }

        private void EnsureApiKeyLoaded()
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                try
                {
                    // Try to get from API config first
                    _apiKey = _apiConfigService.GetGoogleMapsApiKey();
                    Console.WriteLine($"GoogleMapsConfigService loaded API Key from API: {(!string.IsNullOrEmpty(_apiKey) ? "CONFIGURED" : "NOT CONFIGURED")}");
                }
                catch (Exception)
                {
                    // Fall back to static configuration
                    _apiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;
                    Console.WriteLine($"GoogleMapsConfigService loaded API Key from static config: {(!string.IsNullOrEmpty(_apiKey) ? "CONFIGURED" : "NOT CONFIGURED")}");
                }

                if (string.IsNullOrEmpty(_apiKey))
                {
                    Console.WriteLine("Warning: Google Maps API Key not configured. Maps functionality will be disabled.");
                }
            }
        }

        public async Task LoadGoogleMapsAsync()
        {
            if (_isLoaded) return;
            
            EnsureApiKeyLoaded();
            
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
        public string ApiKey 
        { 
            get 
            { 
                EnsureApiKeyLoaded(); 
                return _apiKey; 
            } 
        }
    }
}