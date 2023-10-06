﻿using Projector.Core.Persons.DTO;
using Projector.Core.Users.DTO;
using Projector.Data;
using System.Web;

namespace Projector.Core.Persons
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IPersonsService _personsService;
        private readonly IUsersService _usersService;

        public CreatePersonCommandHandler(ProjectorDbContext context, IPersonsService personsService, IUsersService usersService)
        {
            _db = context;
            _personsService = personsService;
            _usersService = usersService;
        }
        public async Task<CommandResult> Execute(CreatePersonCommand args)
        {
            var duplicateEmail = _db.Users.UserNameExists(args.NewPerson.UserName);
            if(duplicateEmail)
            {
                return CommandResult.Error("Email Address already exists.");
            }

            User newUser = new User
            {
                UserName = args.NewPerson.UserName,
                Password = _usersService.HashPassword(args.NewPerson.UserName+DateTime.Now.ToString()),
                IsVerified = false
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            Person newPerson = new Person
            {
                FirstName = args.NewPerson.FirstName,
                LastName = args.NewPerson.LastName,
                UserId = newUser.Id,
                IsDeleted = false
            };

            _db.Persons.Add(newPerson);
            await _db.SaveChangesAsync();

            //GENERATE VERIFICATION LINK
            newUser.VerificationLink = _usersService.GenerateVerificationLink(newUser.UserName, newUser.Id);

            _db.Users.Update(newUser);
            await _db.SaveChangesAsync();

            //TODO:
            //SEND EMAIL

            return CommandResult.Success(newPerson);
        }
    }
}
