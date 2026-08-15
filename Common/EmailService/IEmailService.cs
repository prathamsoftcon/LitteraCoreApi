namespace LitteraCore.Common.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string message, bool throwOnFailure = false);
    }
}
