using System;

namespace misas_thai_street_cuisine_2._0.EmailTemplates
{
    public static class ErrorNotificationEmail
    {
        public static EmailContent Create(ErrorNotificationData data)
        {
            return new EmailContent
            {
                Subject = $"🚨 Error in {data.Location}",
                HtmlBody = CreateHtml(data),
                PlainTextBody = CreateText(data)
            };
        }

        private static string CreateHtml(ErrorNotificationData data)
        {
            var stackTraceHtml = !string.IsNullOrEmpty(data.StackTrace)
                ? $@"<h3>Stack Trace</h3>
                    <pre style='background-color: #f5f5f5; padding: 15px; border-radius: 4px; overflow-x: auto; font-size: 12px;'>{data.StackTrace}</pre>"
                : "";

            var additionalInfoHtml = !string.IsNullOrEmpty(data.AdditionalInfo)
                ? $@"<h3>Additional Information</h3>
                    <p style='background-color: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 20px 0;'>{data.AdditionalInfo}</p>"
                : "";

            return $@"
<html>
<body style='font-family: Arial, sans-serif; max-width: 700px; margin: 0 auto; padding: 20px;'>
    <div style='background-color: #dc3545; color: white; padding: 20px; border-radius: 8px 8px 0 0;'>
        <h1 style='margin: 0; font-size: 24px;'>🚨 Application Error</h1>
    </div>
    
    <div style='background-color: #f8f9fa; padding: 20px; border: 1px solid #dee2e6; border-top: none;'>
        <h2 style='color: #dc3545; margin-top: 0;'>Error Details</h2>
        
        <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
            <tr>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6; font-weight: bold; width: 150px;'>Location:</td>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6;'>{data.Location}</td>
            </tr>
            <tr>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6; font-weight: bold;'>Integration:</td>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6;'>{data.IntegrationName}</td>
            </tr>
            <tr>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6; font-weight: bold;'>Timestamp:</td>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6;'>{data.Timestamp:yyyy-MM-dd HH:mm:ss UTC}</td>
            </tr>
            <tr>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6; font-weight: bold;'>Error Type:</td>
                <td style='padding: 10px; background-color: white; border: 1px solid #dee2e6;'>{data.ErrorType}</td>
            </tr>
        </table>

        <h3>Error Message</h3>
        <div style='background-color: #f8d7da; color: #721c24; padding: 15px; border-left: 4px solid #dc3545; margin: 20px 0;'>
            {data.ErrorMessage}
        </div>

        {additionalInfoHtml}
        {stackTraceHtml}
    </div>
    
    <div style='background-color: #e9ecef; padding: 15px; border: 1px solid #dee2e6; border-top: none; border-radius: 0 0 8px 8px; text-align: center; font-size: 12px; color: #6c757d;'>
        This is an automated error notification from Misa's Thai Street Cuisine
    </div>
</body>
</html>";
        }

        private static string CreateText(ErrorNotificationData data)
        {
            var stackTrace = !string.IsNullOrEmpty(data.StackTrace)
                ? $"\n\n========== STACK TRACE ==========\n{data.StackTrace}\n================================="
                : "";

            var additionalInfo = !string.IsNullOrEmpty(data.AdditionalInfo)
                ? $"\n\n========== ADDITIONAL INFO ==========\n{data.AdditionalInfo}\n======================================"
                : "";

            return $@"
🚨 APPLICATION ERROR

========== ERROR DETAILS ==========
Location:     {data.Location}
Integration:  {data.IntegrationName}
Timestamp:    {data.Timestamp:yyyy-MM-dd HH:mm:ss UTC}
Error Type:   {data.ErrorType}
===================================

ERROR MESSAGE:
{data.ErrorMessage}
{additionalInfo}
{stackTrace}

---
This is an automated error notification from Misa's Thai Street Cuisine
";
        }
    }

    public record ErrorNotificationData(
        string Location,
        string IntegrationName,
        string ErrorType,
        string ErrorMessage,
        DateTime Timestamp,
        string? StackTrace = null,
        string? AdditionalInfo = null
    );
}
