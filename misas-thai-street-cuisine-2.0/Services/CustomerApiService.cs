using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class CustomerApiService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private string _apiBaseUrl = string.Empty;

        public CustomerApiService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiBaseUrl"] ?? "";
            if (!_apiBaseUrl.EndsWith("/")) _apiBaseUrl += "/";
        }

        public async Task<List<CustomerDto>> GetCustomersAsync() =>
            await _http.GetFromJsonAsync<List<CustomerDto>>($"{_apiBaseUrl}customers");

        public async Task<CustomerDto> GetCustomerByUuidAsync(string uuid) =>
            await _http.GetFromJsonAsync<CustomerDto>($"{_apiBaseUrl}customers/{uuid}");

        public async Task<string> CreateCustomerAsync(CustomerCreateDto customer)
        {
            var response = await _http.PostAsJsonAsync($"{_apiBaseUrl}customers", customer);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateResponse>();
            return result?.uuid ?? string.Empty;
        }

        public class CustomerDto
        {
            public string uuid { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public bool consentToUpdates { get; set; }
            public string createdDttm { get; set; }
        }
        public class CustomerCreateDto
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public bool consentToUpdates { get; set; }
        }
        private class CreateResponse { public string uuid { get; set; } }
    }
}
