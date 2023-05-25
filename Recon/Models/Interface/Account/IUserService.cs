using Recon.Models.Model.Account;
using Recon.ViewModel;

namespace Recon.Models.Interface.Account
{
    public interface IUserService
    {
        // Authenticates a user with the given username and password.
        UserEntity Authenticate(string username, string password);
        // Async Authenticates a user with the given username and password.
        Task<UserEntity> AuthenticateAsync(string username, string password);
        // Get user entity by ID.
        UserEntity GetById(int id);
        /// Create a User Enity
        UserEntity Create(UserEntity user);
        // Register a user to db 
        void Register(RegisterViewModel user);
        // Get User entity by user Name
        UserEntity GetByName(string name);
        // Change password for given userid
        void ChangePassword(int userId, string newPassword);
        // Update user email
        void UpdateUser(int userId, UserEntity updatedUser);
        // Check if user is authenticated 
        bool IsAuthenticated();
        // Given userid get all user Role
        List<Roles> GetRolesForUser(int userId);
        // LogOut user 
        void LogOut();
        // Check if user in Role
        bool IsInRole(string Name);
        // Get user username
        string GetUserName();
        // get user id 
        int GetUserId();
        // get user fullname by id
        string GetFullName(int userid);
        // get uaser Fullname
        string GetFullName();
        /// get user personal infomation by id 
        Person UserGetPersonalInfo(int userId);
        // Update personal information
        void UserUpdatePersonalInfo(Person model);


    }
}
