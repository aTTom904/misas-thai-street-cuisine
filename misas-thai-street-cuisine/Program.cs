using misas_thai_street_cuisine.Components;
using Services;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.Bootstrap;
using Square;
using Square.Models;
using Square.Apis;
using Square.Exceptions;
using Square.Authentication;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace misas_thai_street_cuisine
{
    public class Program
    {
        private static ISquareClient client;
        private static IConfigurationRoot config;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddSingleton<ShoppingCartService>();

            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                })
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();

            builder.Services.AddBlazorBootstrap();

            // Configure Square client
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            config = configBuilder.Build();
            var accessToken = config["AppSettings:AccessToken"];

            client = new SquareClient.Builder()
                .BearerAuthCredentials(
                    new BearerAuthModel.Builder(
                        accessToken
                    ).Build())
                .Environment(Square.Environment.Sandbox)
                .Build();

            builder.Services.AddSingleton<ISquareClient>(client);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();

            // Retrieve locations
            RetrieveLocationsAsync().Wait();
        }

        static async Task RetrieveLocationsAsync()
        {
            try
            {
                ListLocationsResponse response = await client.LocationsApi.ListLocationsAsync();
                foreach (Location location in response.Locations)
                {
                    Console.WriteLine("location:\n  country =  {0} name = {1}",
                        location.Country, location.Name);
                }
            }
            catch (ApiException e)
            {
                var errors = e.Errors;
                var statusCode = e.ResponseCode;
                var httpContext = e.HttpContext;
                Console.WriteLine("ApiException occurred:");
                Console.WriteLine("Headers:");
                foreach (var item in httpContext.Request.Headers)
                {
                    // Display all the headers except Authorization
                    if (item.Key != "Authorization")
                    {
                        Console.WriteLine("\t{0}: \t{1}", item.Key, item.Value);
                    }
                }
                Console.WriteLine("Status Code: \t{0}", statusCode);
                foreach (Error error in errors)
                {
                    Console.WriteLine("Error Category:{0} Code:{1} Detail:{2}", error.Category, error.Code, error.Detail);
                }

                // Your error handling code
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred");
                // Your error handling code
            }
        }
    }
}
