using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Projector.Core.Persons.DTO;
using Projector.Core.Users.DTO;
using Projector.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;

namespace Projector.Core.Users
{
    public class UsersService : IUsersService
    {
        private readonly ProjectorDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(ProjectorDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /* METHODS */
        public bool UserExists(int userId)
        {
            return _db.Users.Any(u => u.Id == userId);
        }

        public User GetUser(int userId)
        {
            return _db.Users.Where(u => u.Id == userId)
                .Include(u => u.VerificationLink)
                .FirstOrDefault();
        }

        public bool VerificationLinkValid(int id, string token)
        {
            var verLink = _db.VerificationLinks
                .Where(v => v.Id == id)
                .FirstOrDefault();

            if (verLink == null)
            {
                return false;
            }

            var timeNow = DateTime.Now;
            if (timeNow > verLink.ExpiryDate)
            {
                return false;
            }

            return verLink.ActivationLink.Equals(token) ?
                true : false;
        }

        public bool VerificationTokenValid(int id, string token)
        {
            var verLink = _db.VerificationLinks
                .Where(v => v.Id == id)
                .FirstOrDefault();

            if (verLink == null)
            {
                return false;
            }

            var timeNow = DateTime.Now;
            if (timeNow > verLink.ExpiryDate)
            {
                return false;
            }

            return verLink.ActivationToken.Equals(token) ?
                true : false;
        }

        public bool VerifyRegistration(int id, string link, string subcode)
        {
            var verLink = _db.VerificationLinks
                .Where(v => v.Id == id)
                .FirstOrDefault();

            if (verLink == null)
            {
                return false;
            }

            var timeNow = DateTime.Now;
            if (timeNow > verLink.ExpiryDate)
            {
                return false;
            }

            return verLink.ActivationLink.Equals(link) && verLink.ActivationToken.Substring(0,10).Equals(subcode) ?
                true : false;
        }
    }
}
