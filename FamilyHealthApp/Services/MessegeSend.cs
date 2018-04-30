using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;


namespace FamilyHealthApp.Services
{
    // Class that handles sending emails
    public class MessegeSend : IEmailSend
    {
        // Options for API key & password
        public MessegeSenderOptions Options { get; set; }

        // Options is saved thru User Secrets
        // Getting them thru this 
        public MessegeSend(IOptions<MessegeSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        // Method for sending the email
        public async Task SendEmailAsync(string email, string subject, string messege)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("support@familyhealthapp.net"),
                Subject = subject,
                Body = messege
           };

            mailMessage.To.Add(email);

            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential(Options.SendGridApiKey, Options.SendGridAzurePassword),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            // Since it's async, we wrap the send method around this
            await Task.Run(() =>
            {
                smtpClient.Send(mailMessage);
            });
        }
    }
}