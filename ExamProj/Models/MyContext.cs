using ExamProj.Models;
using Microsoft.EntityFrameworkCore;
 
namespace ExamProj.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get; set;}
        public DbSet<Occasion> Occasions {get; set;}
        public DbSet<Attend> Attends {get; set;}
    }
}