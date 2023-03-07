using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recon.Models;

namespace Recon.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<MagneticCard> magneticCards { get; set; }
        public DbSet<CheckInWork> Checks { get; set; }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<UsersInRoles> UsersInRole { get; set; }

        public DbSet<Person> Person { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MagneticCard>().HasKey(entity => new { entity.CardId, entity.UserId });
            builder.Entity<UsersInRoles>().HasKey(entity => new { entity.roleId, entity.userId });
            builder.Entity<UsersInRoles>().HasKey(entity => new { entity.userId });
        }
    }
    
}

