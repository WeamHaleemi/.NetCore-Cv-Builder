using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {


        }
        public DbSet<User> UserTable { get; set; }
        public DbSet<Nationality> NationalityTable { get; set; }
        public DbSet<Skill> SkillsTable { get; set; }
        
    }
}
