using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recon.Controllers.api;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Tests.Mock.Service.User;

namespace Tests
{
    public class ListRoleControllerTests
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly ListRoleController _controller;
        public ListRoleControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDb")
              .Options;

            _dbContext = new DataDbContext(options);

            // Create a mock IUserService object
            _userService = new MockUserService();

            // Create an instance of the ListGroupController class
            _controller = new ListRoleController(_dbContext, _userService);
        }

        [Fact]
        public void Get_ReturnsListOfRoles()
        {
            // Arrange
            var roles = new List<Roles>()
            {
                new Roles { Id = 3, Name = "Role 1" },
                new Roles { Id = 4, Name = "Role 2" },
                new Roles { Id = 5, Name = "Role 3" },
                new Roles { Id = 6, Name = "Role 4" }
            };
            _dbContext.Role.AddRange(roles);
            _dbContext.Role.Count();
            _dbContext.SaveChanges();


            // Set up the mock IUserService object to return true for IsAuthenticated method
            ((MockUserService)_userService).IsAuthenticatedResult = true;

            // Act
            var result = _controller.Get();


            // Assert
            Assert.IsType<ActionResult<List<Roles>>>(result);
            var data = Assert.IsAssignableFrom<List<Roles>>(result.Value);
            Assert.Equal(4, data.Count());
        }

        [Fact]
        public void Get_ReturnsListOfRolesNotAuthenticated()
        {
            // Arrange



            // Set up the mock IUserService object to return true for IsAuthenticated method
            ((MockUserService)_userService).IsAuthenticatedResult = false;

            // Act
            var result = _controller.Get();


            // Assert

            //Assert.IsType<ActionResult<Unauthorized>>(result);                       
            Assert.True(result.Value.IsNullOrEmpty());
        }

    }
}
