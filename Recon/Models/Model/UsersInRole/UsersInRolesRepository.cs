using Recon.Data;
using Recon.Models.Model.Account;

namespace Recon.Models.Repository
{
    public class UsersInRolesRepository : IUsersInRolesRepository
    {
        private readonly DataDbContext _dbContext;

        public UsersInRolesRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<UsersInRoles> GetAll()
        {
            return _dbContext.UsersInRole.ToList();
        }

        public UsersInRoles GetById(int roleId, int userId)
        {
            return _dbContext.UsersInRole.Find(roleId, userId);
        }

        public void Add(UsersInRoles usersInRoles)
        {
            
            _dbContext.UsersInRole.Add(usersInRoles);
            _dbContext.SaveChanges();
        }

        public void Update(UsersInRoles usersInRoles)
        {
            _dbContext.UsersInRole.Update(usersInRoles);
            _dbContext.SaveChanges();
        }

        public void Delete(int roleId, int userId)
        {
            var usersInRoles = GetById(roleId, userId);
            if (usersInRoles != null)
            {
                _dbContext.UsersInRole.Remove(usersInRoles);
                _dbContext.SaveChanges();
            }
        }
    }
}
