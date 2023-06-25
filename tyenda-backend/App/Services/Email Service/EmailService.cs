using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace tyenda_backend.App.Services.Email_Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string bodyType, string body)
        {
            
            // Create a new MimeMessage
            var message = new MimeMessage();

            var from = _configuration["EmailConfig:username"];
            var password = _configuration["EmailConfig:password"];

            // Set the From address
            message.From.Add(new MailboxAddress("Tyenda Team", from));

            // Set the To address
            message.To.Add(new MailboxAddress("Account", to));

            // Set the subject
            message.Subject = subject;

            // Set the body
            message.Body = new TextPart(bodyType)
            {
                Text = body,
            };

            var host = _configuration["EmailConfig:host"];
            var port = _configuration["EmailConfig:port"];

            // Create a new SmtpClient
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(host, Int32.Parse(port!), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(from, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        } 
    }
}