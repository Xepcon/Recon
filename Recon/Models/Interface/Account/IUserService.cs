using Recon.Models.Model.Account;

namespace Recon.Models.Interface.Account
{
    public interface IUserService
    {
        UserEntity Authenticate(string username, string password);
        UserEntity GetById(int id);
        UserEntity Create(UserEntity user);

        UserEntity GetByName(string name);
        void ChangePassword(int userId, string newPassword);
        void UpdateUser(int userId, UserEntity updatedUser);
        bool IsAuthenticated();
        List<Roles> GetRolesForUser(int userId);
        void LogOut();

        bool IsInRole(string Name);
    }
}
