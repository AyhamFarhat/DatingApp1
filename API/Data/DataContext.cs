// it is a class that will be used to connect to the database and it will be used to create the tables in the database
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options){}
    public DbSet<AppUser> Users { get; set; }

}
