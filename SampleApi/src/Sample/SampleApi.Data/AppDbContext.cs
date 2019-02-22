using Microsoft.EntityFrameworkCore;
using SampleApi.Core;

namespace SampleApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
         public DbSet<State> States {get;set;}

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
