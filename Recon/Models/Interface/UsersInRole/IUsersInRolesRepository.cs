using Recon.Models.Model.Account;

namespace Recon.Models.Repository
{
    public interface IUsersInRolesRepository
    {
        // Get all UsersInRole
        IEnumerable<UsersInRoles> GetAll();
        // get UsersInRole by roleid and userid
        UsersInRoles GetById(int roleId, int userId);
        // add UsersInRole to db 
        void Add(UsersInRoles usersInRoles);
        // Update UsersInRole in db 
        void Update(UsersInRoles usersInRoles);
        // Delete UsersInRole from db 
        void Delete(int roleId, int userId);
    }
}
