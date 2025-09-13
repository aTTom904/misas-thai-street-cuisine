using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MisasThaiApi.Services;
using System.Text.Json;

namespace MisasThaiApi.Functions;

public class HealthFunctions
{
    private readonly ILogger _logger;
    private readonly string _connectionString;

    public HealthFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<HealthFunctions>();
        _connectionString = Environment.GetEnvironmentVariable("SqlConnectionString") ?? "";
    }

    [Function("HealthCheck")]
    public async Task<HttpResponseData> HealthCheck(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        try
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            
            var health = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Database = "Not Tested"
            };

            // Test database connection if connection string is available
            if (!string.IsNullOrEmpty(_connectionString))
            {
                var dbService = new DatabaseService(_connectionString);
                var isDbHealthy = await dbService.TestConnectionAsync();
                health = health with { Database = isDbHealthy ? "Connected" : "Connection Failed" };
            }

            await response.WriteStringAsync(JsonSerializer.Serialize(health, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }));

            response.Headers.Add("Content-Type", "application/json");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            var error = new
            {
                Status = "Unhealthy",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };

            await errorResponse.WriteStringAsync(JsonSerializer.Serialize(error));
            errorResponse.Headers.Add("Content-Type", "application/json");
            return errorResponse;
        }
    }

    [Function("DatabaseStatus")]
    public async Task<HttpResponseData> DatabaseStatus(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "database/status")] HttpRequestData req)
    {
        try
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                var noConnResponse = req.CreateResponse(HttpStatusCode.ServiceUnavailable);
                await noConnResponse.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    Status = "No Connection String",
                    Message = "Database connection string not configured"
                }));
                return noConnResponse;
            }

            var dbService = new DatabaseService(_connectionString);
            
            // Test connection
            var isConnected = await dbService.TestConnectionAsync();
            
            if (!isConnected)
            {
                var failResponse = req.CreateResponse(HttpStatusCode.ServiceUnavailable);
                await failResponse.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    Status = "Connection Failed",
                    Message = "Cannot connect to database"
                }));
                return failResponse;
            }

            // Test tables
            try
            {
                await dbService.InitializeDatabaseAsync();
                
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    Status = "Healthy",
                    Message = "Database connected and tables exist",
                    Timestamp = DateTime.UtcNow
                }));
                response.Headers.Add("Content-Type", "application/json");
                return response;
            }
            catch (InvalidOperationException ex)
            {
                var tablesResponse = req.CreateResponse(HttpStatusCode.ServiceUnavailable);
                await tablesResponse.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    Status = "Tables Missing",
                    Message = ex.Message
                }));
                return tablesResponse;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database status check failed");
            
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync(JsonSerializer.Serialize(new
            {
                Status = "Error",
                Message = ex.Message
            }));
            return errorResponse;
        }
    }
}