namespace Projector.Core
{
    public interface IAuthenticationService
    {
        //void SignIn(UserData user);
        //void SignOut();
        string GetRoute(string actionName, string controllerName, object routeValues);
    }
    /*
    public class UserData
    {
        public int PersonId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
    }*/
}
