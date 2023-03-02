using Microsoft.EntityFrameworkCore;
using Recon.Models;

namespace Recon.Data
{
    public class DataDbContext:DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base() {
        
        
        }
        public DbSet<UserEntity> Users { get; set; }
    }
}
