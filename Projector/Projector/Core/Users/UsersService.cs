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
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8, HashAlgorithmName.SHA256))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public bool VerifyPassword(string hashed, string input)
        {
            byte[] buffer4;
            byte[] src = Convert.FromBase64String(hashed);
            if (src.Length != 0x31 || src[0] != 0)
            {
                return false;
            }

            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(input, dst, 0x3e8, HashAlgorithmName.SHA256))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return buffer3.SequenceEqual(buffer4);
        }

        public string GenerateActivationToken(string email, DateTime timeNow)
        {
            return HashPassword(email + timeNow.ToString());
        }

        public async Task PersistLogin(PersonData user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("PersonId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
                );
        }

        public async Task RemoveLogin()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

        }

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

        public bool isVerified(int userId)
        {
            return _db.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault()
                .IsVerified;
        }

        public VerificationLink GenerateVerificationLink(string userName, int id, int type)
        {
            var timeNow = DateTime.Now.AddDays(1);
            string activationToken = GenerateActivationToken(userName, timeNow);

            // TYPE: 1 needs password, 0 already has password
            string route = type == 1 ?
                    "/projector/resetpassword/" :
                    "/projector/verify/";

            string url = "https://localhost:7125" + route + id + "?v=" + HttpUtility.UrlEncode(activationToken);

            return new VerificationLink
            {
                Id = id,
                ActivationToken = activationToken,
                ActivationLink = url,
                ExpiryDate = timeNow
            };
        }
    }
}
