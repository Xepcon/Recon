using Recon.Models.Model.Account;

namespace Recon.Models.Repository
{
    public interface IUsersInRolesRepository
    {
        IEnumerable<UsersInRoles> GetAll();
        UsersInRoles GetById(int roleId, int userId);
        void Add(UsersInRoles usersInRoles);
        void Update(UsersInRoles usersInRoles);
        void Delete(int roleId, int userId);
    }
}
