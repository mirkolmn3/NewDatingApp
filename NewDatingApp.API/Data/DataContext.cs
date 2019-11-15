

using Microsoft.EntityFrameworkCore;
using NewDatingApp.API.Models;

namespace NewDatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            
        }

        public DbSet<Value> Values { get; set; }


    }
}