using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class MenuItemApiService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private string _apiBaseUrl = string.Empty;

        public MenuItemApiService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiBaseUrl"] ?? "";
            if (!_apiBaseUrl.EndsWith("/")) _apiBaseUrl += "/";
        }

        public async Task<List<MenuItemDto>> GetMenuItemsAsync() =>
            await _http.GetFromJsonAsync<List<MenuItemDto>>($"{_apiBaseUrl}menuitems");

        public async Task<MenuItemDto> GetMenuItemByUuidAsync(string uuid) =>
            await _http.GetFromJsonAsync<MenuItemDto>($"{_apiBaseUrl}menuitems/{uuid}");

        public async Task<string> CreateMenuItemAsync(MenuItemCreateDto item)
        {
            var response = await _http.PostAsJsonAsync($"{_apiBaseUrl}menuitems", item);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateResponse>();
            return result?.uuid ?? string.Empty;
        }

        public class MenuItemDto
        {
            public string uuid { get; set; }
            public string itemName { get; set; }
            public string category { get; set; }
            public decimal price { get; set; }
            public int quantity { get; set; }
            public string createdDttm { get; set; }
            public string updatedDttm { get; set; }
        }
        public class MenuItemCreateDto
        {
            public string itemName { get; set; }
            public string category { get; set; }
            public decimal price { get; set; }
            public int quantity { get; set; }
        }
        private class CreateResponse { public string uuid { get; set; } }
    }
}
