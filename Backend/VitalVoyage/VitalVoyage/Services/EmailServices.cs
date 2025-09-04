using MailKit.Net.Smtp;
using MimeKit;
using System.Runtime;
using VitalVoyage.Models.DTOs;
using VitalVoyage.Services.Contracts;

namespace VitalVoyage.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _config;
        public EmailServices(IConfiguration configuration)
        {
            _config = configuration;
        }
        public async Task SendEmail(EmailDTO verifyEmailDTO)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_config["EmailSettings:SenderEmail"]);
            email.From.Add(new MailboxAddress(_config["EmailSettings:SenderName"], _config["EmailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(verifyEmailDTO.To));
            email.Subject = verifyEmailDTO.Subject;

            Console.WriteLine(email.To);

            var builder = new BodyBuilder()
            {
                HtmlBody = verifyEmailDTO.Body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
