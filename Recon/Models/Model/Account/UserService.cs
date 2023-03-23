using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.ViewModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Recon.Models.Model.Account
{
    public class UserService : IUserService
    {
        private readonly DataDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(DataDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public string GetUserName() {
            if (IsAuthenticated())
            {
                var userId = GetUserId();
                //var user = GetById(userId);
                var personEntity = _dbContext.Person.Find(userId);
                if (personEntity == null)
                {
                    return null;
                }
                else {
                    if(personEntity.FirstName!=null && personEntity.LastName != null) {
                        return personEntity.FirstName + " " + personEntity.LastName;
                    }
                    return null;
                    
                }                

            }
            else
            {
                return null;
            }


        }
        public bool IsInRole(string Name)
        {
            if (IsAuthenticated())
            {
                var userId = GetUserId();
                var user = GetById(userId);
                var userRolesWithNames = from ur in _dbContext.UsersInRole
                                         join r in _dbContext.Role on ur.roleId equals r.Id
                                         where ur.userId == userId
                                         select r.Name;

                if (userRolesWithNames.IsNullOrEmpty())
                {
                    return false;
                }
                else
                {
                    return userRolesWithNames.Contains(Name);
                }
            }
            else {
                return false;
            }
        }
        public bool IsAuthenticated()
        {
            // Check if the UserId session key exists
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            return !string.IsNullOrEmpty(userId);
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var user = GetById(userId);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _dbContext.SaveChanges();
        }


        public void UpdateUser(int userId, UserEntity updatedUser)
        {
            var user = GetById(userId);
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;

            _dbContext.SaveChanges();
        }
        public UserEntity Authenticate(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                Debug.WriteLine("NULL USER ");
                return null;
            }

            _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("Username", user.Username);
            Debug.WriteLine("NOT NULL USER ");
            return user;
        }

        public void LogOut()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public UserEntity GetByName(string name)
        {
            if (IsAuthenticated())
            {
                return _dbContext.Users.FirstOrDefault(x => x.Username == name);
            }
            else {
                return null;
            }
            
        }
        public UserEntity GetById(int id)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public UserEntity Create(UserEntity user)
        {
            // check if username is already taken
            if (_dbContext.Users.Any(x => x.Username == user.Username))
                throw new ApplicationException("Username '" + user.Username + "' is already taken");



            // hash the password before storing in the database
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);


            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;

        }

        public List<Roles> GetRoles() {
            var roles = new List<Roles>();

            // Retrieve the user from the database
            int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
            if (userId == null) {
                return roles;
            }
            else
            {
                var user = GetById(userId);

                if (user != null)
                {
                    // Retrieve the roles for the user
                    roles = _dbContext.UsersInRole
                        .Where(x => x.userId == user.Id)
                        .Join(_dbContext.Role, uir => uir.roleId, r => r.Id, (uir, r) => r)
                        .ToList();
                }

                return roles;
            }
           
        }

        public string GetFullName(int userid) {
            var model = _dbContext.Person.Where(x=>x.userId==userid).FirstOrDefault();
            if (model != null)
            {
                return model.FirstName + " " + model.LastName;
            }
            return "";
        }

        public string GetFullName()
        {
            var model = _dbContext.Person.Where(x => x.userId == GetUserId()).FirstOrDefault();
            if (model != null)
            {
                return model.FirstName + " " + model.LastName;
            }
            return "";
        }


       
        public List<Roles> GetRolesForUser(int userId)
        {
            var roles = new List<Roles>();

            // Retrieve the user from the database
            var user = GetById(userId);

            if (user != null)
            {
                // Retrieve the roles for the user
                roles = _dbContext.UsersInRole
                    .Where(x => x.userId == user.Id)
                    .Join(_dbContext.Role, uir => uir.roleId, r => r.Id, (uir, r) => r)
                    .ToList();
            }

            return roles;
        }

        public int GetUserId() {
            if (IsAuthenticated())
            {
                var userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
                return userId;
            }
            else {
                return -1;
            }
        }

        public void Register(RegisterViewModel model) {
                UserEntity user = new UserEntity
                {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Email = model.Email,
                };

           
                Create(user);
                
                var tempororaryPersonId = new Person();
                tempororaryPersonId.userId = user.Id;
                _dbContext.Person.Add(tempororaryPersonId);
                _dbContext.SaveChanges();
                
               
           


        }
        public Person UserGetPersonalInfo(int userId) {
            return _dbContext.Person.Where(x => x.userId == userId).FirstOrDefault();
        }

        public void UserUpdatePersonalInfo(Person model)
        {
            if (model != null)
            {
                _dbContext.Person.Update(model);
                _dbContext.SaveChanges();
            }
        }

    }
}
