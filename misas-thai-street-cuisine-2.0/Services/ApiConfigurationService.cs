using System.Text.Json;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class ApiConfigurationService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiConfigurationService> _logger;
        private readonly IConfiguration _staticConfiguration;
        private ApiConfigurationData? _configData;
        private bool _isLoaded = false;

        public ApiConfigurationService(ILogger<ApiConfigurationService> logger, IConfiguration staticConfiguration)
        {
            // Create our own HttpClient instance to avoid DI scope issues
            _httpClient = new HttpClient();
            _logger = logger;
            _staticConfiguration = staticConfiguration;
        }

        public async Task LoadConfigurationAsync()
        {
            if (_isLoaded && _configData != null)
                return;

            try
            {
                _logger.LogInformation("Loading configuration from API...");
                
                // Get the base API URL from static config first (as fallback)
                // This will use the ApiBaseUrl from appsettings.json during development
                // and should be updated to your Azure Functions URL for production
                var apiBaseUrl = _staticConfiguration["ApiBaseUrl"] ?? "http://localhost:7071/api";
                
                var response = await _httpClient.GetAsync($"{apiBaseUrl}/GetConfig");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    _configData = JsonSerializer.Deserialize<ApiConfigurationData>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    _isLoaded = true;
                    _logger.LogInformation("Configuration loaded successfully from API");
                }
                else
                {
                    _logger.LogError($"Failed to load configuration from API. Status: {response.StatusCode}");
                    throw new InvalidOperationException($"Failed to load configuration: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading configuration from API");
                throw new InvalidOperationException("Failed to load configuration from API", ex);
            }
        }

        public string GetSquareApplicationId()
        {
            EnsureConfigLoaded();
            return _configData?.Square?.ApplicationId ?? throw new InvalidOperationException("Square ApplicationId not configured");
        }

        public string GetSquareEnvironment()
        {
            EnsureConfigLoaded();
            return _configData?.Square?.Environment ?? "sandbox";
        }

        public string GetGoogleMapsApiKey()
        {
            EnsureConfigLoaded();
            return _configData?.GoogleMaps?.ApiKey ?? string.Empty;
        }

        public string GetApiBaseUrl()
        {
            EnsureConfigLoaded();
            return _configData?.Api?.BaseUrl ?? throw new InvalidOperationException("API BaseUrl not configured");
        }

        private void EnsureConfigLoaded()
        {
            if (!_isLoaded || _configData == null)
            {
                throw new InvalidOperationException("Configuration has not been loaded. Call LoadConfigurationAsync() first.");
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    public class ApiConfigurationData
    {
        public SquareConfig? Square { get; set; }
        public GoogleMapsConfig? GoogleMaps { get; set; }
        public ApiConfig? Api { get; set; }
    }

    public class SquareConfig
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string Environment { get; set; } = "sandbox";
    }

    public class GoogleMapsConfig
    {
        public string ApiKey { get; set; } = string.Empty;
    }

    public class ApiConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}