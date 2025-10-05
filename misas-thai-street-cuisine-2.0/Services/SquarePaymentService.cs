using Microsoft.JSInterop;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class SquarePaymentService
    {

        
        private readonly IJSRuntime _jsRuntime;
        private readonly string _applicationId = "sq0idp-vBWKF52a4zCjMaxzbKHJTw";
        private readonly string _locationId = "LHWQP128FDFZF";

        public SquarePaymentService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<PaymentInitResult> InitializeAsync()
        {
            var result = await _jsRuntime.InvokeAsync<PaymentInitResult>(
                "squarePayments.init", _applicationId, _locationId);
            return result;
        }

        public async Task<PaymentInitResult> InitializeCardAsync(string containerId)
        {
            var result = await _jsRuntime.InvokeAsync<PaymentInitResult>(
                "squarePayments.initCard", containerId);
            return result;
        }

        public async Task<TokenizeResult> TokenizeCardAsync()
        {
            var result = await _jsRuntime.InvokeAsync<TokenizeResult>(
                "squarePayments.tokenizeCard");
            return result;
        }

        public async Task DestroyAsync()
        {
            await _jsRuntime.InvokeVoidAsync("squarePayments.destroy");
        }
    }

    public class PaymentInitResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class TokenizeResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public object? Details { get; set; }
        public object[]? Errors { get; set; }
        public string? Error { get; set; }
    }
}