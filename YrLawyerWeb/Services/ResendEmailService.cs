using Resend;

namespace YrLawyerWeb.Services
{
    public interface IEmailService
    {
        public Task Send(string toEmail, string title, string htmlBody);
    }

    public class ResendEmailService(ILogger<ResendEmailService> logger) : IEmailService
    {
        private readonly ILogger<ResendEmailService> _logger = logger;
        public async Task Send(string toEmail, string title, string htmlBody)
        {
            IResend resend = ResendClient.Create("re_QQwH8BCH_Dd9BNwKVYUhHaW9ZDvU7Q1EC");

            var fromEmail = "YrLawyer <onboarding@resend.dev>";

            var resp = await resend.EmailSendAsync(new EmailMessage()
            {
                From = fromEmail,
                To = toEmail,
                Subject = title,
                HtmlBody = htmlBody
            });

            _logger.LogInformation($"Email send to: {toEmail}\ntitle: {title}\ndata:{htmlBody}\nWith Guid: {resp.Content}\nFrom {fromEmail}");
        }
    }
}
