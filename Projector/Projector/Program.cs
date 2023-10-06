using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Projector.Core;
using Projector.Core.Persons;
using Projector.Core.Persons.DTO;
using Projector.Core.Projects;
using Projector.Core.Projects.DTO;
using Projector.Core.Users;
using Projector.Core.Users.DTO;
using Projector.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ALLOW ACCESS TO HTTP CONTEXT
builder.Services.AddHttpContextAccessor();

// ADD DATABASE SERVICE
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProjectorDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// DEPENDENCY INJECTION of services
builder.Services.AddScoped<ProjectorDbContext>(sp => new ProjectorDbContext());
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICommandBus, CommandBus>();

// REGISTRATION OF COMMAND HANDLERS
builder.Services.AddScoped<ICommandHandler<SignInCommand>, SignInCommandHandler>();
builder.Services.AddScoped<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateProjectCommand>, CreateProjectCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreatePersonCommand>, CreatePersonCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EditProjectCommand>, EditProjectCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteProjectCommand>, DeleteProjectCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EditPersonCommand>, EditPersonCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeletePersonCommand>, DeletePersonCommandHandler>();
builder.Services.AddScoped<ICommandHandler<AssignPersonCommand>, AssignPersonCommandHandler>();
builder.Services.AddScoped<ICommandHandler<RemovePersonCommand>, RemovePersonCommandHandler>();
builder.Services.AddScoped<ICommandHandler<ResetPasswordCommand>, ResetPasswordCommandHandler>();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/Forbidden/";
            options.LoginPath = "/projector/signin";
            options.LogoutPath = "/projector/signout";
        });
// BUILDING THE APP
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCookiePolicy(new CookiePolicyOptions(){
    MinimumSameSitePolicy = SameSiteMode.Strict
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
