﻿using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Interface.Account;
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

        public bool IsInRole(string Name)
        {
            if (IsAuthenticated())
            {
                var userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
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
            return _dbContext.Users.FirstOrDefault(x => x.Username == name);
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

    }
}
