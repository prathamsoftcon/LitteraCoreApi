using System.Threading.Tasks;
namespace LitteraCore.Common.SmsService
{
    public interface ISmsService
    {
        Task SendSmsAsync(string recipientMobile, string message, string templateId, bool throwOnFailure = false);
        LitteraCore.Models.SmsSettings.SmsTemplate GetTemplateMsg(int templateType);
    }
}
