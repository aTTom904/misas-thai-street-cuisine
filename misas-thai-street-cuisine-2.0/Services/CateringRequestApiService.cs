using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using misas_thai_street_cuisine_2._0.Services;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class CateringRequestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CateringRequestApiService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApiConfigurationService _apiConfigService;
        private string _apiBaseUrl = string.Empty;

        public CateringRequestApiService(HttpClient httpClient, ILogger<CateringRequestApiService> logger, IConfiguration configuration, ApiConfigurationService apiConfigService)
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
                    _apiBaseUrl = _apiConfigService.GetApiBaseUrl();
                }
                catch (Exception)
                {
                    _apiBaseUrl = _configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("API Base URL not configured");
                }
            }
        }

        public async Task<ApiResponse<bool>> SubmitCateringRequestAsync(object dto)
        {
            try
            {
                EnsureApiBaseUrlLoaded();
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/CateringRequestsApi", dto);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool> { Success = true, Data = true };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API Error: {response.StatusCode} - {errorContent}");
                    return new ApiResponse<bool> { Success = false, Error = $"API Error: {response.StatusCode} - {errorContent}" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting catering request");
                return new ApiResponse<bool> { Success = false, Error = $"Network error: {ex.Message}" };
            }
        }
    }
}
