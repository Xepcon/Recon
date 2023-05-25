using Recon.Models.Model.Account;

namespace Recon.Models.Repository
{
    public interface IRolesRepository
    {
        /// return all roles 
        IEnumerable<Roles> GetAllRoles();
        // get role by id from db 
        Roles GetRoleById(int id);
        // add role to db 
        void AddRole(Roles role);
        // Update role in db 
        void UpdateRole(Roles role);
        // Delete role from db 
        void DeleteRole(int id);
    }
}
