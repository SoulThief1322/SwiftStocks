using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace SwiftStocks.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(_emailSettings.FromEmail))
                throw new InvalidOperationException("FromEmail is not configured or empty.");

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.FromEmail);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            mailMessage.IsBodyHtml = true;

			var smtp = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort);
			smtp.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
			smtp.EnableSsl = true;

			await smtp.SendMailAsync(mailMessage);

		}
    }

}