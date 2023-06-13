using InternetShop.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InternetShop.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        private readonly IConfiguration _config;

        public EmailSender(ILogger<EmailSender> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public Task SendEmailAsync(string email, string subject, string messageHTML)
        {
            string emailShop = _config.GetValue<string>("EmailConfiguration:Email");
            string passwordShop = _config.GetValue<string>("EmailConfiguration:Password");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(emailShop);
            message.To.Add(email);
            message.Subject = subject;
            message.Body = messageHTML;
            message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailShop, passwordShop);
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);

            return Task.CompletedTask;
        }
    }
}
