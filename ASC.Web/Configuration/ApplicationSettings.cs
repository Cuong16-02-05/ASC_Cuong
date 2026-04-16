namespace ASC.Web.Configuration
{
    public class ApplicationSettings
    {
        public string? Title { get; set; }
        public string? AdminEmail { get; set; }
        public string? AdminName { get; set; }
        public string? AdminPassword { get; set; }
        public string? EngineerEmail { get; set; }
        public string? EngineerName { get; set; }
        public string? EngineerPassword { get; set; }
        public SmtpConfig? Smtp { get; set; }
    }

    public class SmtpConfig
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? From { get; set; }
        public string? Password { get; set; }
    }
}
