using Microsoft.EntityFrameworkCore;
using Recon.Models.Model.Account;
using Recon.Models.Model.Card;
using Recon.Models.Model.GroupLib;
using Recon.Models.Model.Ticket;
using Recon.Models.Model.TimeManager;

namespace Recon.Data
{
    public class DataDbContext : DbContext, IDataDbContext
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
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceEntity> AttendanceEntitys { get; set; }

        public DbSet<DayOffTicket> DayOffTicket { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<MagneticCard>().HasNoKey();
            builder.Entity<MagneticCard>().HasKey(entity => new { entity.CardId, entity.userId });
            builder.Entity<UsersInRoles>().HasKey(entity => new { entity.roleId, entity.userId });

            builder.Entity<AttendanceEntity>().HasKey(entity => new { entity.AttendanceId, entity.AttendanceDate });
            builder.Entity<GroupMember>().HasKey(entity => new { entity.groupId, entity.userId });
            builder.Entity<Group>().HasKey(entity => new { entity.groupId });
            builder.Entity<DayOffTicket>().HasKey(entity => new { entity.Id });
        }

        public int SaveChanges()
        {
            return base.SaveChanges();
        }
    }

}

