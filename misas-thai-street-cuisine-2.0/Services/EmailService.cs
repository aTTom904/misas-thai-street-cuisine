using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using misas_thai_street_cuisine_2._0.EmailTemplates;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class EmailService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly ApiConfigurationService _apiConfigService;
        private string _apiBaseUrl = string.Empty;

        public EmailService(
            HttpClient http, 
            IConfiguration configuration, 
            ApiConfigurationService apiConfigService,
            ILogger<EmailService> logger)
        {
            _http = http;
            _configuration = configuration;
            _apiConfigService = apiConfigService;
            _logger = logger;
            
            EnsureApiBaseUrlLoaded();
        }

        private void EnsureApiBaseUrlLoaded()
        {
            if (!string.IsNullOrEmpty(_apiBaseUrl)) return;

            try
            {
                _apiBaseUrl = _apiConfigService.GetApiBaseUrl();
                if (!string.IsNullOrEmpty(_apiBaseUrl))
                {
                    _logger.LogInformation("Email Service API URL loaded from API configuration");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load API URL from API config, falling back to static config");
            }

            _apiBaseUrl = _configuration["Api:BaseUrl"] ?? string.Empty;
            
            if (string.IsNullOrEmpty(_apiBaseUrl))
            {
                throw new InvalidOperationException("API Base URL not configured in any source");
            }

            _logger.LogInformation("Email Service API URL loaded from static configuration");
        }

        public async Task<EmailSendResult> SendEmailAsync(string to, EmailContent content, string? replyTo = null)
        {
            try
            {
                _logger.LogInformation("Sending email to {Recipient}", to);

                var request = new EmailRequest
                {
                    To = to,
                    ReplyTo = replyTo,
                    Subject = content.Subject,
                    HtmlBody = content.HtmlBody,
                    PlainTextBody = content.PlainTextBody
                };

                var response = await _http.PostAsJsonAsync($"{_apiBaseUrl}SendEmail", request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent successfully to {Recipient}", to);
                    return new EmailSendResult { Success = true };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send email. Status: {StatusCode}, Error: {Error}", 
                        response.StatusCode, errorContent);
                    
                    return new EmailSendResult 
                    { 
                        Success = false, 
                        ErrorMessage = $"Failed to send email: {response.StatusCode}" 
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending email to {Recipient}", to);
                return new EmailSendResult 
                { 
                    Success = false, 
                    ErrorMessage = ex.Message 
                };
            }
        }
    }

    public class EmailSendResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
