using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class Context : DbContext
    {
        //public DbSet<Electrician>? Electricians { get; set; }
        //public DbSet<Owner>? Owners { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Location>? Locations { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
