using System.Text;
using API.Data;
using API.interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API.Extensions;
using API.Middleware; // to allow us using  AddApplicationServices(...)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // the code in /Extensions/AddApplicationServices.cs
builder.Services.AddIdentityServices(builder.Configuration);// the code in /Extensions/AddIdentityServices.cs

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200") ); // to help angular to access

app.UseAuthentication(); // asks  : do you have a valid token
app.UseAuthorization(); // if you have.... says : ok you have a valid token.... now what are you allowed to do

app.MapControllers();



using var scope = app.Services.CreateScope();
var services=scope.ServiceProvider;
try{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}catch(Exception ex){
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}



app.Run();
