using Microsoft.EntityFrameworkCore;
using STTechUserManagement.Models;

namespace STTechUserManagement.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}