using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;
using Square;
using Square.Models;
using Square.Authentication;
using Azure.Communication.Email;
using System.Linq;

namespace MisasThaiStreetCuisine.Function
{
    public static class TakePayment
    {
        [Function("TakePayment")]
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext)
        {
            var log = executionContext.GetLogger("TakePayment");
            log.LogInformation("Processing payment request...");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var orderRequest = JsonConvert.DeserializeObject<CreateOrderRequest>(requestBody);

            var accessToken = System.Environment.GetEnvironmentVariable("Square__AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                log.LogError("Square credentials missing.");
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(new { success = false, error = "Square credentials missing." });
                return response;
            }

            try
            {
                var client = new SquareClient.Builder()
                    .BearerAuthCredentials(
                        new BearerAuthModel.Builder(accessToken).Build()
                    )
                    .Environment(Square.Environment.Sandbox)
                    .Build();

                var money = new Money(
                    amount: (long)(orderRequest.Total * 100),
                    currency: "USD"
                );

                var paymentRequest = new CreatePaymentRequest(
                    sourceId: orderRequest.PaymentToken,
                    idempotencyKey: Guid.NewGuid().ToString(),
                    amountMoney: money
                );

                var response = await client.PaymentsApi.CreatePaymentAsync(paymentRequest);
                if (response.Payment != null && response.Payment.Status == "COMPLETED")
                {
                    // Send email notification
                    await SendOrderConfirmationEmail(orderRequest, response.Payment.Id, log);
                    
                    var successResponse = req.CreateResponse(HttpStatusCode.OK);
                    await successResponse.WriteAsJsonAsync(new { 
                        success = true, 
                        orderNumber = response.Payment.Id, 
                        message = "Payment processed successfully." 
                    });
                    return successResponse;
                }
                else
                {
                    var failResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await failResponse.WriteAsJsonAsync(new { success = false, error = "Payment failed.", details = response.Errors });
                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing payment.");
                var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await errorResponse.WriteAsJsonAsync(new { success = false, error = ex.Message });
                return errorResponse;
            }
        }

        private static async Task SendOrderConfirmationEmail(CreateOrderRequest order, string orderNumber, ILogger log)
        {
            try
            {
                log.LogInformation("Preparing to send order confirmation email...");
                var connectionString = System.Environment.GetEnvironmentVariable("AzureCommunicationServices__ConnectionString");
                var fromEmail = System.Environment.GetEnvironmentVariable("AzureCommunicationServices__FromEmail");
                var replyToEmail = System.Environment.GetEnvironmentVariable("AzureCommunicationServices__ReplyToEmail");
                
                // Debug logging to see actual values
                log.LogInformation($"Connection String: {(string.IsNullOrEmpty(connectionString) ? "null or empty" : connectionString.Length > 20 ? connectionString.Substring(0, 20) + "..." : connectionString)}");
                log.LogInformation($"From Email: {fromEmail}");
                log.LogInformation($"Reply-To Email: {replyToEmail}");
                
                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(fromEmail))
                {
                    log.LogWarning("Email configuration missing. Skipping email notification.");
                    return;
                }

                // Try basic client creation first
                log.LogInformation("Creating EmailClient...");
                var emailClient = new EmailClient(connectionString);
                log.LogInformation("Email client initialized.");

                // Test with minimal email first
                log.LogInformation("Creating simple email message...");
                var emailMessage = new EmailMessage(
                    senderAddress: fromEmail,
                    recipientAddress: order.CustomerEmail,
                    content: new EmailContent("Test Order Confirmation")
                    {
                        PlainText = "This is a test email from Misa's Thai Street Cuisine.",
                        Html = "<p>This is a test email from Misa's Thai Street Cuisine.</p>"
                    });

                log.LogInformation("Attempting to send email...");
                // Send to customer
                var customerEmailResult = await emailClient.SendAsync(Azure.WaitUntil.Completed, emailMessage);
                log.LogInformation("Email send completed successfully.");

                // Restaurant email temporarily disabled for testing
                // var restaurantEmail = System.Environment.GetEnvironmentVariable("Restaurant__NotificationEmail");
                // if (!string.IsNullOrEmpty(restaurantEmail)) { ... }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to send email notification. Exception details: {ExceptionType}: {ExceptionMessage}", ex.GetType().Name, ex.Message);
                if (ex.InnerException != null)
                {
                    log.LogError("Inner exception: {InnerExceptionType}: {InnerExceptionMessage}", ex.InnerException.GetType().Name, ex.InnerException.Message);
                }
                // Don't throw - email failure shouldn't break the payment process
            }
        }

        private static string CreateOrderEmailHtml(CreateOrderRequest order, string orderNumber)
        {
            var itemsHtml = string.Join("", order.Items.Select(item => 
                $"<tr><td>{item.ItemName}</td><td>{item.Quantity}</td><td>${item.Price:F2}</td><td>${(item.Price * item.Quantity):F2}</td></tr>"));

            var consentText = order.ConsentToUpdates 
                ? "Yes - You will receive promotional emails and text messages about special offers, new menu items, and restaurant updates." 
                : "No - You will not receive promotional communications.";

            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                <h2 style='color: #ee6900;'>Thank you for your order!</h2>
                <p>Hi {order.CustomerName},</p>
                <p>Your order has been confirmed and payment processed successfully.</p>
                
                <h3>Order Details</h3>
                <p><strong>Order Number:</strong> {orderNumber}</p>
                <p><strong>Total:</strong> ${order.Total:F2}</p>
                
                <h3>Delivery Details</h3>
                <p><strong>Delivery Address:</strong> {order.DeliveryAddress}</p>
                <p><strong>Delivery Date:</strong> {order.DeliveryDate}</p>
                
                <h3>Items Ordered</h3>
                <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                    <thead>
                        <tr style='background-color: #f8f9fa;'>
                            <th style='border: 1px solid #ddd; padding: 8px; text-align: left;'>Item</th>
                            <th style='border: 1px solid #ddd; padding: 8px; text-align: center;'>Qty</th>
                            <th style='border: 1px solid #ddd; padding: 8px; text-align: right;'>Price</th>
                            <th style='border: 1px solid #ddd; padding: 8px; text-align: right;'>Total</th>
                        </tr>
                    </thead>
                    <tbody>
                        {itemsHtml}
                    </tbody>
                </table>
                
                <h3>Marketing Preferences</h3>
                <p><strong>Marketing Communications:</strong> {consentText}</p>
                {(order.ConsentToUpdates ? "<p style='font-size: 12px; color: #666;'><em>You can unsubscribe at any time. Standard message and data rates may apply for text messages.</em></p>" : "")}
                
                <p>We'll be in touch with delivery updates!</p>
                
                <h3>Questions or Comments?</h3>
                <p>If you have any questions or comments about your order, please don't hesitate to reach out to us at <a href='mailto:msthaistreetcuisine@gmail.com' style='color: #ee6900;'>msthaistreetcuisine@gmail.com</a>. We're here to help!</p>
                
                <p>Thank you for choosing Misa's Thai Street Cuisine!</p>
                
                <hr style='margin: 30px 0;'>
                <p style='font-size: 12px; color: #666;'>
                    Misa's Thai Street Cuisine<br>
                    1301 N Orange Ave, Suite 102<br>
                    Green Cove Springs, FL 32043<br>
                    904.315.4884
                </p>
            </body>
            </html>";
        }

        private static string CreateOrderEmailText(CreateOrderRequest order, string orderNumber)
        {
            var itemsText = string.Join("\n", order.Items.Select(item => 
                $"- {item.ItemName} (Qty: {item.Quantity}) - ${item.Price:F2} each = ${(item.Price * item.Quantity):F2}"));

            var consentText = order.ConsentToUpdates 
                ? "Yes - You will receive promotional emails and text messages about special offers, new menu items, and restaurant updates." 
                : "No - You will not receive promotional communications.";

            return $@"
Thank you for your order!

Hi {order.CustomerName},

Your order has been confirmed and payment processed successfully.

Order Details:
- Order Number: {orderNumber}
- Total: ${order.Total:F2}

Delivery Details:
- Delivery Address: {order.DeliveryAddress}
- Delivery Date: {order.DeliveryDate}

Items Ordered:
{itemsText}

Marketing Preferences:
- Marketing Communications: {consentText}
{(order.ConsentToUpdates ? "You can unsubscribe at any time. Standard message and data rates may apply for text messages." : "")}

We'll be in touch with delivery updates!

Questions or Comments?
If you have any questions or comments about your order, please don't hesitate to reach out to us at msthaistreetcuisine@gmail.com. We're here to help!

Thank you for choosing Misa's Thai Street Cuisine!

---
Misa's Thai Street Cuisine
1301 N Orange Ave, Suite 102
Green Cove Springs, FL 32043
904.315.4884";
        }
        // Model for deserializing order request
        public class CreateOrderRequest
        {
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerPhone { get; set; }
            public bool ConsentToUpdates { get; set; }
            public string DeliveryAddress { get; set; }
            public string DeliveryDate { get; set; }
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