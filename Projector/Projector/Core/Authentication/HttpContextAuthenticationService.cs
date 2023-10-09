namespace Projector.Core.Authentication
{
    public class HttpContextAuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public HttpContextAuthenticationService(IHttpContextAccessor httpContextAccessor, LinkGenerator generator)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = generator;
        }

        /*public void SignIn(UserData user)
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }*/

        public string GetRoute(string actionName, string controllerName, object routeValues)
        {
            var path = _linkGenerator.GetPathByAction(actionName, controllerName, routeValues);

            var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            var host = _httpContextAccessor.HttpContext.Request.Host;

            return scheme + "://" + host + path;
        }
    }
}
