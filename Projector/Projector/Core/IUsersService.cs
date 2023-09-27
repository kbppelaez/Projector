﻿using Projector.Core.Users.DTO;

namespace Projector.Core
{
    public interface IUsersService
    {
        bool VerifyPassword(string hashed, string input);
        string HashPassword(string password);
        Task PersistLogin(UserData user);
    }
}