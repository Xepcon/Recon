
using Microsoft.EntityFrameworkCore;
using Recon.Models.Model.Account;
using Recon.Models.Model.Card;
using Recon.Models.Model.GroupLib;
using Recon.Models.Model.Ticket;
using Recon.Models.Model.TimeManager;

namespace Recon.Data
{
    public interface IDataDbContext : IDisposable
    {
        public DbSet<MagneticCard> magneticCards { get; set; }
        public DbSet<CheckInWork> Checks { get; set; }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<UsersInRoles> UsersInRole { get; set; }

        public DbSet<Person> Person { get; set; }

       

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceEntity> AttendanceEntitys { get; set; }

        public DbSet<DayOffTicket> DayOffTicket { get; set; }
        int SaveChanges();
    }
}
