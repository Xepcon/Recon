using Recon.Data;
using Recon.Models.Model.Account;

namespace Recon.Models.Repository
{
    public class RolesRepository:IRolesRepository
    {
        private readonly DataDbContext _dbContext;

        public RolesRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Roles> GetAllRoles()
        {
            return _dbContext.Role;
        }
        public Roles GetRoleById(int id)
        {
            return _dbContext.Role.Find(id);
        }
        public void AddRole(Roles role)
        {
            _dbContext.Add(role);
            _dbContext.SaveChanges();
        }
        public void UpdateRole(Roles role)
        {
            _dbContext.Update(role);
            _dbContext.SaveChanges();
        }
        public void DeleteRole(int id)
        {
            var role = _dbContext.Role.Find(id);
            if (role != null)
            {
                _dbContext.Role.Remove(role);
                _dbContext.SaveChanges();
            }
        }
    
    }
}
