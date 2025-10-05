using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Square;
using Square.Payments;


namespace MisasThaiStreetCuisine.Function
{
    public static class TakePayment
    {
        [FunctionName("TakePayment")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing payment request...");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var orderRequest = JsonConvert.DeserializeObject<CreateOrderRequest>(requestBody);

            var accessToken = Environment.GetEnvironmentVariable("Square__AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                log.LogError("Square credentials missing.");
                return new BadRequestObjectResult(new { success = false, error = "Square credentials missing." });
            }

            try
            {
                var client = new SquareClient(
                    token: accessToken,
                    clientOptions: new ClientOptions{
                        BaseUrl = SquareEnvironment.Sandbox
                    }
                );
                log.LogInformation("Square client initialized with access token: " + (string.IsNullOrEmpty(accessToken) ? "null or empty" : accessToken));

                var money = new Money
                {
                    Amount = (long)(orderRequest.Total * 100),
                    Currency = Currency.Usd
                };

                var paymentRequest = new CreatePaymentRequest
                {
                    SourceId = orderRequest.PaymentToken,
                    IdempotencyKey = Guid.NewGuid().ToString(),
                    AmountMoney = money,
                };

                var response = await client.Payments.CreateAsync(paymentRequest);
                if (response.Payment != null && response.Payment.Status == "COMPLETED")
                {
                    return new OkObjectResult(new { success = true, orderNumber = response.Payment.Id, message = "Payment processed successfully." });
                }
                else
                {
                    return new BadRequestObjectResult(new { success = false, error = "Payment failed.", details = response.Errors });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing payment.");
                return new BadRequestObjectResult(new { success = false, error = ex.Message });
            }
        }
        // Model for deserializing order request
        public class CreateOrderRequest
        {
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerPhone { get; set; }
            public bool ConsentToUpdates { get; set; }
            public string DeliveryAddress { get; set; }
            public decimal Total { get; set; }
            public string PaymentToken { get; set; }
            public System.Collections.Generic.List<OrderItemRequest> Items { get; set; }
        }

        public class OrderItemRequest
        {
            public string ItemName { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}