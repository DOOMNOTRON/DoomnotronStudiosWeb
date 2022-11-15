using SendGrid.Helpers.Mail;
using SendGrid;

namespace DoomnotronStudiosWeb.Models
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(string toEmail, string fromEmail, string subject, 
                                string content, string htmlContent);
    }

    public class EmailProviderSendGrid : IEmailProvider
    {
        public readonly IConfiguration _config;
        public EmailProviderSendGrid(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string toEmail, string fromEmail, string subject, string content, string htmlContent)
        {
            // SendGrid Email Test
            var apiKey = _config.GetSection("SendGridKey").Value;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("meownster4life@gmail.com", "Big Meownsters!"),
                Subject = "Testing",
                PlainTextContent = "This is a test Email",
                HtmlContent = "<strong>This is a test Email</strong>"
            };
            msg.AddTo(new EmailAddress("meownster4life@gmail.com", "Little Meownsters!"));
            await client.SendEmailAsync(msg);
            //var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            // END SENDGRID TES
        }
    }
}
