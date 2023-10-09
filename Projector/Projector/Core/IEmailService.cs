using Projector.Core.Email;

namespace Projector.Core
{
    public interface IEmailService
    {
        void SendEmail(EmailContentData email);
    }
}
