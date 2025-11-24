using misas_thai_street_cuisine_2._0.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using misas_thai_street_cuisine_2._0;
using MudBlazor.Services;
using misas_thai_street_cuisine_2._0.Data;
using Square;
using Square.Locations;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add configuration from wwwroot
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", optional: true, reloadOnChange: true);

builder.Services.AddSingleton<OrderContextService>();
builder.Services.AddSingleton<DeadlineService>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<misas_thai_street_cuisine_2._0.Services.ShoppingCartService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register API Configuration Service
builder.Services.AddSingleton<ApiConfigurationService>();

// Register MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddSingleton<MenuData>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<SquarePaymentService>();
builder.Services.AddScoped<OrderApiService>();
builder.Services.AddScoped<DeliveryValidationService>();
builder.Services.AddScoped<GoogleMapsConfigService>();
builder.Services.AddScoped<AddressVerificationApiService>();
builder.Services.AddScoped<CustomerApiService>();
builder.Services.AddScoped<MenuItemApiService>();
var app = builder.Build();

// Load configuration from API during startup
try
{
    var configService = app.Services.GetRequiredService<ApiConfigurationService>();
    await configService.LoadConfigurationAsync();
}
catch (Exception ex)
{
    // Log error but don't prevent app from starting - fall back to static config
    Console.WriteLine($"Warning: Failed to load API configuration: {ex.Message}");
}

// Configure manual delivery dates (uncomment and modify as needed)
var deadlineService = app.Services.GetRequiredService<DeadlineService>();
deadlineService.SetManualDeliveryDates(
    new DateTime(2025, 12, 17), // First delivery date
    new DateTime(2025, 12, 31)  // Second delivery date
);

await app.RunAsync();
