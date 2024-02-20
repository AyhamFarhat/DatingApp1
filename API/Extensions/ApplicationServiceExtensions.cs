
using API.Data;
using API.interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>{
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            }); //adding DataContext as a service

        services.AddCors();// to help angular to access
        services.AddScoped<ITokenService, TokenService>();
        return services;
        }

    }
}