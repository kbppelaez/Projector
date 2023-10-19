namespace Projector.Core.Users.DTO
{
    public class ForgotPasswordCommand
    {
        public string EmailAddress { get; set; }
        public string ForgotPasswordBaseUrl { get; set; }
    }
}
