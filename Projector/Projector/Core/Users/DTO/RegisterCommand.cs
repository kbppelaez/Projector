namespace Projector.Core.Users.DTO
{
    public class RegisterCommand
    {
        public NewUserData Details { get; set; }
        public string RegisterUserBaseUrl { get; set; }
    }
}
