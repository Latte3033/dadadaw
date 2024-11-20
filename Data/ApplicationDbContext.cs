using Microsoft.EntityFrameworkCore;
using WebStock.Models;

namespace WebStock.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<EmailDraft> EmailDraft { get; set; }
    }
}
