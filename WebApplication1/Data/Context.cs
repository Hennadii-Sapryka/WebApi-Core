using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class Context : DbContext
    {
        public DbSet<Electrician>? Electricians { get; set; }
        public DbSet<Skill>? Skills { get; set; }
        public DbSet<Feedback>? Feedbacks { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

    }
}
