using Microsoft.EntityFrameworkCore;
using TestRestAPI.Data.Models;

namespace TestRestAPI.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {
                
        }


        public DbSet<Category> Categories { get; set; }

    }
}
