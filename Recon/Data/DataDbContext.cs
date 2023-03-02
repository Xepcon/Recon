using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recon.Models;

namespace Recon.Data
{
    public class DataDbContext : IdentityDbContext<UserEntity>
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<MagneticCard> magneticCards { get; set; }
        public DbSet<CheckInWork> Checks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MagneticCard>().HasKey(entity => new { entity.CardId, entity.UserId });
        }
    }
    
}

