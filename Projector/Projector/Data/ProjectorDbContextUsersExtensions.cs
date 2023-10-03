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

        public static bool CodeOrProjectNameExists(this DbSet<Project> project, string pName, string pCode)
        {
            return project.Any(p => p.Code == pCode || p.Name == pName);
        }

        public static bool CodeExists(this DbSet<Project> project, string pCode)
        {
            return project.Any(p => p.Code == pCode);
        }

        public static bool ProjectNameExists(this DbSet<Project> project, string pName)
        {
            return project.Any(p => p.Name == pName);
        }
    }
}
