using System.Text;
using API.Data;
using API.interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using API.Entities;
using API.SignalR; // to allow us using  AddApplicationServices(...)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // the code in /Extensions/AddApplicationServices.cs
builder.Services.AddIdentityServices(builder.Configuration);// the code in /Extensions/AddIdentityServices.cs

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("https://localhost:4200") ); // to help angular to access

app.UseAuthentication(); // asks  : do you have a valid token
app.UseAuthorization(); // if you have.... says : ok you have a valid token.... now what are you allowed to do

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");



using var scope = app.Services.CreateScope();
var services=scope.ServiceProvider;
try{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();

    // to solve multiple connection between the users
    await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
    context.Connections.RemoveRange(context.Connections);

    await Seed.SeedUsers(userManager, roleManager);
}catch(Exception ex){
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}



app.Run();
