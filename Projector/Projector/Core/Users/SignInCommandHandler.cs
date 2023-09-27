﻿using Microsoft.EntityFrameworkCore;
using Projector.Core;
using Projector.Core.Users.DTO;
using Projector.Data;

namespace Projector.Core.Users
{
    public class SignInCommandHandler : ICommandHandler<SignInCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;

        public SignInCommandHandler(ProjectorDbContext projectorDbContext, IUsersService usersService) {
            _db = projectorDbContext;
            _usersService = usersService;
        }
        public async Task<CommandResult> Execute(SignInCommand user)
        {
            if (!string.IsNullOrEmpty(user.Details.UserName))
            {
                var existingUser = await _db.Users
                    .FirstOrDefaultAsync(u => u.UserName == user.Details.UserName);

                if(existingUser == null)
                {
                    return CommandResult.Error("Invalid credentials given.");
                }

                bool passwordMatched = _usersService.VerifyPassword(existingUser.Password, user.Details.Password);

                if (passwordMatched)
                {
                    // TODO:
                    // Check if account is verified
                    
                    // IF NOT VERIFIED
                        //IF LINK EXPIRED
                            //return CommandResult.Error("Account is not verified. Please contact your administrator.");
                        //IF LINK NOT EXPIRED
                            //return CommandResult.Error("Account is not verified. Please check your email for the verification link.");

                    // IF VERIFIED:
                    return CommandResult.Success(existingUser);
                }

                return CommandResult.Error("Invalid credentials given.");
            }

            return CommandResult.Error("Invalid credentials given.");            
        }
    }
}
