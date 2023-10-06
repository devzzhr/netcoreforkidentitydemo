
using Microsoft.EntityFrameworkCore;
using Even.Models;


namespace Even.Data
{
    public class BaseContactDBContext : DbContext
    {
        public BaseContactDBContext(DbContextOptions<BaseContactDBContext> options)
           : base(options)
        {
        }
        public DbSet<BaseContact> BaseContacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseContact>().ToTable("BaseContact");
            
        }
    }
}

