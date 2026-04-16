using ASC.Web.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ASC.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class AuthMessageSender : IEmailSender
    {
        private readonly ApplicationSettings _settings;

        public AuthMessageSender(IOptions<ApplicationSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("ASC", _settings.Smtp?.From));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };

                using var client = new SmtpClient();
                await client.ConnectAsync(_settings.Smtp?.Host, _settings.Smtp?.Port ?? 587,
                    MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Smtp?.From, _settings.Smtp?.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log error - email sending failed but don't crash the app
                Console.WriteLine($"Email send failed: {ex.Message}");
            }
        }
    }
}
