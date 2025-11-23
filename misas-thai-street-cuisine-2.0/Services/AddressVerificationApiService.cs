using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class AddressVerificationApiService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private string _apiBaseUrl = string.Empty;

        public AddressVerificationApiService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiBaseUrl"] ?? "";
            if (!_apiBaseUrl.EndsWith("/")) _apiBaseUrl += "/";
        }

        public async Task<List<AddressVerificationDto>?> GetAddressVerificationsAsync() =>
            await _http.GetFromJsonAsync<List<AddressVerificationDto>>($"{_apiBaseUrl}addressverification");

        public async Task<AddressVerificationDto?> GetAddressVerificationByUuidAsync(string uuid) =>
            await _http.GetFromJsonAsync<AddressVerificationDto>($"{_apiBaseUrl}addressverification/{uuid}");

        public async Task<string?> CreateAddressVerificationAsync(AddressVerificationCreateDto av)
        {
            var response = await _http.PostAsJsonAsync($"{_apiBaseUrl}addressverification", av);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateResponse>();
            return result?.uuid;
        }

        public class AddressVerificationDto
        {
            public string? uuid { get; set; }
            public string? address { get; set; }
            public bool addressVerified { get; set; }
            public string? data { get; set; }
            public string? createTime { get; set; }
            public string? updateTime { get; set; }
        }
        public class AddressVerificationCreateDto
        {
            public string? address { get; set; }
            public bool addressVerified { get; set; }
            public string? data { get; set; }
        }
        private class CreateResponse { public string? uuid { get; set; } }
    }
}
