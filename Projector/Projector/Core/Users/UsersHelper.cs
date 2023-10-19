using Projector.Data;
using System.Security.Cryptography;
using System.Security.Policy;

namespace Projector.Core.Users
{
    public static class UsersHelper
    {

        public static string HashPassword(string password)
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

        public static bool VerifyPassword(string hashed, string input)
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

        public static VerificationLink GenerateVerificationLink(string username, int id)
        {
            var timeNow = DateTime.Now;
            var expiry = timeNow.AddDays(1);

            string activationToken = GenerateActivationToken(username, timeNow);
            string activationLink = GenerateActivationToken(username, expiry);

            return new VerificationLink
            {
                Id = id,
                ActivationToken = activationToken,
                ActivationLink = activationLink,
                ExpiryDate = expiry
            };
        }

        public static string GenerateActivationToken(string username, DateTime time)
        {
            return HashPassword(username + time.ToString());
        }

        public static string CreateLink(string route, string token)
        {
            return route + "?v=" + token;
        }
    }
}
