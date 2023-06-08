using Microsoft.EntityFrameworkCore;
using MyWebApplicationCRUD.Models;

namespace MyWebApplicationCRUD.EmployeeData
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            
        }
     
        public DbSet<EmployeeRecords> EmployeeRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }
}
