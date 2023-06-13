using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ServiceContracts
{
    /// <summary>
    /// Tools for sending emails.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Method for sending email.
        /// </summary>
        /// <param name="email">Emails to send message.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="message">Message to send.</param>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
