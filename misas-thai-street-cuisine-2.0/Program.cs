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
builder.Services.AddSingleton<OrderContextService>();
builder.Services.AddSingleton<DeadlineService>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<misas_thai_street_cuisine_2._0.Services.ShoppingCartService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddSingleton<MenuData>();
builder.Services.AddScoped<SquarePaymentService>();
builder.Services.AddScoped<OrderApiService>();

var app = builder.Build();

// Configure manual delivery dates (uncomment and modify as needed)
var deadlineService = app.Services.GetRequiredService<DeadlineService>();
deadlineService.SetManualDeliveryDates(
    new DateTime(2025, 10, 15), // First delivery date
    new DateTime(2025, 10, 29)  // Second delivery date
);

await app.RunAsync();