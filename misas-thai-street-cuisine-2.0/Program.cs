using misas_thai_street_cuisine_2._0.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using misas_thai_street_cuisine_2._0;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
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

// Register Blazorise services
builder.Services
    .AddBlazorise( options =>
    {
        options.Immediate = true;
    } )
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddSingleton<MenuData>();
builder.Services.AddScoped<SquarePaymentService>();
builder.Services.AddScoped<OrderApiService>();

await builder.Build().RunAsync();