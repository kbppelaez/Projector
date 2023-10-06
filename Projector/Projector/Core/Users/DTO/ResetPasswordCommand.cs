namespace Projector.Core.Users.DTO
{
    public class ResetPasswordCommand
    {
        public PasswordData Password {  get; set; }
        public int UserId { get; set; }
    }
}
