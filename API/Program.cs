using System.Text;
using API.Data;
using API.interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API.Extensions; // to allow us using  AddApplicationServices(...)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // the code in /Extensions/AddApplicationServices.cs
builder.Services.AddIdentityServices(builder.Configuration);// the code in /Extensions/AddIdentityServices.cs

var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200") ); // to help angular to access

app.UseAuthentication(); // asks  : do you have a valid token
app.UseAuthorization(); // if you have.... says : ok you have a valid token.... now what are you allowed to do

app.MapControllers();

app.Run();
