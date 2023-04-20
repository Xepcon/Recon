using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Recon.ViewModel;

namespace Tests.Mock.Service.User
{
    public class MockUserService : IUserService
    {
        public bool IsAuthenticatedResult { get; set; } = true;
        public int UserIDResult { get; set; } = 1;
        public List<Roles> GetRolesForUserResult { get; set; } = new List<Roles>();

        public UserEntity Authenticate(string username, string password)
        {
            return null;
        }

        public Task<UserEntity> AuthenticateAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(int userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public UserEntity Create(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public string GetFullName(int userid)
        {
            throw new NotImplementedException();
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public List<Roles> GetRolesForUser(int userId)
        {
            return GetRolesForUserResult;
        }

        public int GetUserId()
        {
            return UserIDResult;
        }

        public string GetUserName()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated()
        {
            return IsAuthenticatedResult;
        }

        public bool IsInRole(string Name)
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public void Register(RegisterViewModel user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(int userId, UserEntity updatedUser)
        {
            throw new NotImplementedException();
        }

        public Person UserGetPersonalInfo(int userId)
        {
            throw new NotImplementedException();
        }

        public void UserUpdatePersonalInfo(Person model)
        {
            throw new NotImplementedException();
        }
    }
}
