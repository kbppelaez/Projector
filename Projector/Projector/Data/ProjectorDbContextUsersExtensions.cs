using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Projector.Data
{
    public static class ProjectorDbContextUsersExtensions
    {
        public static bool UserNameExists(this DbSet<User> users, string username)
        {
            return users.Any(user => user.UserName == username);
        }

    }
}
