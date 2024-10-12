using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Plan> Plans { get; set; }
    }

}
