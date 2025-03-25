using Domain.Base;
using System.Net;
using System.Net.Mail;
using Application.Interfaces.Email;
using Microsoft.Extensions.Configuration;

namespace Infra.Implementations.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _email;
        private readonly string _password;
        private readonly string _smtpServer;
        private readonly int _smtpPort;

        public EmailService(IConfiguration configuration)
        {
            _email = configuration["EmailConfiguration:Email"];
            _password = configuration["EmailConfiguration:Password"];
            _smtpServer = configuration["EmailConfiguration:SmtpServer"];
            _smtpPort = int.Parse(configuration["EmailConfiguration:SmtpPort"]);
        }

        public async Task<Operation> SendEmail(string to, string subject, string body, bool isHtml)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_email, _password);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_email),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = isHtml
                    };

                    mailMessage.To.Add(to);

                    await client.SendMailAsync(mailMessage);
                }

                return Operation.MakeSuccess();
            }
            catch (Exception ex)
            {
                return Operation.MakeFailure(ex.Message);
            }
        }
    }
}