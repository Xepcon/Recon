using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Recon.Data;
using Recon.Models.Model.Account;
using System.Text;

namespace Recon.Tests.Models.Account
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IDataDbContext> _mockDbContext;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public UserServiceTests()
        {
            _mockDbContext = new Mock<IDataDbContext>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _userService = new UserService(_mockDbContext.Object, _mockHttpContextAccessor.Object);
        }

        [Fact]
        public void Unit_IsAuthenticated_ReturnsFalse_WhenSessionIsNull()
        {
            // Arrange
            var session = new Mock<ISession>();
            session.Setup(x => x.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
                   .Returns(false);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            // Act
            var isAuthenticated = _userService.IsAuthenticated();

            // Assert
            Assert.False(isAuthenticated);
        }
        [Fact]
        public void Unit_Create_AddsUserToDatabase()
        {
            // Arrange
            var user = new UserEntity
            {
                Username = "testuser",
                PasswordHash = "password",
                Email = "testuser@example.com"
            };
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new DataDbContext(options);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var userService = new UserService(dbContext, mockHttpContextAccessor.Object);

            // Act
            var createdUser = userService.Create(user);

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal(user.Username, createdUser.Username);
            Assert.Equal(user.Email, createdUser.Email);
            Assert.NotNull(dbContext.Users.Find(createdUser.Id));
        }

        [Fact]
        public void Unit_GetUserName_ReturnsNull_WhenNotAuthenticated()
        {
            // Arrange
            var session = new Mock<ISession>();
            session.Setup(x => x.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
                   .Returns(false);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            // Act
            var userName = _userService.GetUserName();

            // Assert
            Assert.Null(userName);
        }
        [Fact]

        public void Unit_RolesTest_WhenAuthenticated()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = 1,
                Username = "roleuser",
                PasswordHash = "password",
                Email = "roleuser@example.com"
            };
            var role = new Roles
            {
                Id = 1,
                Name = "Admin"
            };
            var userRole = new UsersInRoles
            {

                userId = user.Id,
                roleId = role.Id
            };
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new DataDbContext(options);
            dbContext.Users.Add(user);
            dbContext.Role.Add(role);
            dbContext.UsersInRole.Add(userRole);
            dbContext.SaveChanges();

            var session = new Mock<ISession>();
            var userIdBytes = Encoding.UTF8.GetBytes(user.Id.ToString());
            session.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            // Act
            var userService = new UserService(dbContext, _mockHttpContextAccessor.Object);
            var isInRole = userService.IsInRole(role.Name);
            var roles = userService.GetRoles();


            // Assert
            Assert.True(roles.Contains(role));
            Assert.True(isInRole);
        }
        [Fact]
        public void Unit_IsInRole_ReturnsFalse_WhenUserIsNotInRole()
        {
            // Arrange
            var userid = 1;
            var role = new Roles
            {
                Id = 2,
                Name = "Hr"
            };

            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new DataDbContext(options);

            dbContext.Role.Add(role);

            dbContext.SaveChanges();

            var session = new Mock<ISession>();
            var userIdBytes = Encoding.UTF8.GetBytes(userid.ToString());
            session.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            // Act
            var userService = new UserService(dbContext, _mockHttpContextAccessor.Object);
            var isInRole = userService.IsInRole(role.Name);

            // Assert
            Assert.False(isInRole);
        }
        [Fact]
        public void Unit_IsAuthenticated_ReturnsTrue_WhenLoggedIn()
        {

            var username = "Authuser";
            var password = "password";
            var user = new UserEntity
            {
                Id = 3,
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Email = "testuser@example.com"
            };
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new DataDbContext(options);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var session = new Mock<ISession>();
            var userIdBytes = Encoding.UTF8.GetBytes(user.Id.ToString());
            session.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var userService = new UserService(dbContext, _mockHttpContextAccessor.Object);

            // Act
            var authenticatedUser = userService.Authenticate(username, password);

            // Assert
            Assert.NotNull(authenticatedUser);
            Assert.Equal(user.Username, authenticatedUser.Username);
            Assert.Equal(user.Email, authenticatedUser.Email);
        }
        [Fact]
        public void Unit_LogOut_ClearsSession()
        {
            // Arrange
            var session = new Mock<ISession>();
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var userService = new UserService(null, httpContextAccessor.Object);

            // Act
            userService.LogOut();

            // Assert
            session.Verify(x => x.Clear(), Times.Once);
        }

        [Fact]

        public void Unit_GetUserId_ReturnsCorrectUserId_WhenAuthenticated()
        {
            // Arrange
            var userId = 1;
            var session = new Mock<ISession>();
            var userIdBytes = Encoding.UTF8.GetBytes(userId.ToString());
            session.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var userService = new UserService(null, mockHttpContextAccessor.Object);

            // Act
            var result = userService.GetUserId();

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact]
        public void Unit_GetFullName_ReturnsCorrectName_WhenUserExists()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new DataDbContext(options);

            var userid = 1;

            var person = new Person
            {

                userId = userid,
                FirstName = "John",
                LastName = "Doe"
            };
            dbContext.Person.Add(person);
            dbContext.SaveChanges();

            var session = new Mock<ISession>();
            var userIdBytes = Encoding.UTF8.GetBytes(userid.ToString());
            session.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var userService = new UserService(dbContext, httpContextAccessor.Object);

            // Act
            var fullName = userService.GetFullName(userid);

            // Assert
            Assert.Equal("John Doe", fullName);
        }

        [Fact]
        public void Unit_GetFullName_WhenNotAuthenticated()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new DataDbContext(options);
            var session = new Mock<ISession>();
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(session.Object);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);


            var userService = new UserService(dbContext, httpContextAccessor.Object);


            // Act
            var fullName = userService.GetFullName();

            // Assert
            Assert.Equal("", fullName);
        }


    }
}