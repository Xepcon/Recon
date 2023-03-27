using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using System.Linq;
using Xunit;


namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
    }

    /*public class UserServiceTests
    {
        private readonly IUserService _userService;
        private DataDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;
        public UserServiceTests()
        {
            // Set up the DbContext and IHttpContextAccessor for the test
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "ReconTestDb")
                .EnableSensitiveDataLogging()
                .Options;
            _dbContext = new DataDbContext(options);
            _httpContextAccessor = new HttpContextAccessor();

            // Create a test user and add them to the DbContext
            var user = new UserEntity { Username = "testuser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpass"), Email = "test@test.com" };
            _dbContext.Users.Add(user);

            var personEntities = new List<Person>
        {
            new Person { userId = 1, FirstName = "John", LastName = "Doe" }
        };
            var roleEntities = new List<Roles>
        {
            new Roles { Id = 1, Name = "Admin" }
        };
            var usersInRoleEntities = new List<UsersInRoles>
        {
            new UsersInRoles { userId = 1, roleId = 1 }
        };
            _dbContext.AddRange(personEntities);
            _dbContext.AddRange(roleEntities);
            _dbContext.AddRange(usersInRoleEntities);
            //DbContext.SaveChanges();

            _dbContext.SaveChanges();

            // Set up the UserService for the test
            _userService = new UserService(_dbContext, _httpContextAccessor);
        }


       

    }*/
}
