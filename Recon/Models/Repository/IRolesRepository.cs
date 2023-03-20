using Recon.Models.Model.Account;

namespace Recon.Models.Repository
{
    public interface IRolesRepository
    {
        IEnumerable<Roles> GetAllRoles();
        Roles GetRoleById(int id);
        void AddRole(Roles role);
        void UpdateRole(Roles role);
        void DeleteRole(int id);
    }
}
