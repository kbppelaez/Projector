using Projector.Core.Email;

namespace Projector.Core
{
    public interface IEmailService
    {
        void SendEmail(EmailContentData email);
        string GetRoute(string actionName, string controllerName, object routeValues);
    }
}
