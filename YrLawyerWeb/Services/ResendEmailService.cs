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
            IResend resend = ResendClient.Create("re_7V7eD3Kt_6rQ1F6FaB72q4GPoSTKksvab");

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
