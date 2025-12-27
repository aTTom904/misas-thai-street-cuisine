namespace misas_thai_street_cuisine_2._0.EmailTemplates
{
    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string? ReplyTo { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string HtmlBody { get; set; } = string.Empty;
        public string PlainTextBody { get; set; } = string.Empty;
    }
}
