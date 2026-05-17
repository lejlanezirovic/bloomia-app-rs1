using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Abstractions;
using Bloomia.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bloomia.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
            logger.LogInformation("Email password configured: {HasPassword}", !string.IsNullOrWhiteSpace(_settings.Password));
        }

        public async Task SendAppointmentBookingConfirmationAsync(string toEmail, string clientName, string therapistName,
            DateOnly appointmentDate, TimeOnly appointmentTime, string sessionType, CancellationToken ct)
        {

            var subject = "Appointment booking confirmation";

            var body = $"Hello {clientName}, \n\n" +
                $"      Your appointment has been successfully booked. \n\n" +
                $"" +
                $"      Therapist: {therapistName}\n" +
                $"      Date: {appointmentDate:dd.MM.yyyy}\n" +
                $"      Time: {appointmentTime:HH\\:mm}\n" +
                $"      Session type: {sessionType}\n\n" +
                $"      Thank you,\n" +
                $"      Bloomia";

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            message.To.Add(toEmail);

            using var smtpClient = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.UseSsl
            };

            _logger.LogInformation("Sending booking confirmation email to {Recipient}", toEmail);

            ct.ThrowIfCancellationRequested();
            await smtpClient.SendMailAsync(message);

            _logger.LogInformation("Booking confirmation email succesfully sent to {Recipient}", toEmail);
        
        }
    }
}
