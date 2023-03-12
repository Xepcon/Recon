using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recon.Models.Model.Account;
using Recon.Models.Model.Card;
using Recon.Models.Model.Group;
using Recon.Models.Model.TimeManager;

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

        public DbSet<WorkTimeUsers> WorkTimeUsers { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }    

        public DbSet<Group> Groups { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MagneticCard>().HasKey(entity => new { entity.CardId, entity.UserId });
            builder.Entity<UsersInRoles>().HasKey(entity => new { entity.roleId, entity.userId });
            
            builder.Entity<WorkTimeUsers>().HasKey(entity => new { entity.UserId });
            builder.Entity<GroupMember>().HasKey(entity => new { entity.groupId,entity.userId });
            builder.Entity<Group>().HasKey(entity => new { entity.groupId});
        }
    }
    
}

