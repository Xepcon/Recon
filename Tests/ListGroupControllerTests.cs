using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Recon.Controllers.api;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Recon.Models.Model.GroupLib;
using Recon.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Mock.Service.User;

namespace Tests
{
    public class ListGroupControllerTests
    {

        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly ListGroupController _controller;
        public ListGroupControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDb")
              .Options;

            _dbContext = new DataDbContext(options);

            // Create a mock IUserService object
            _userService = new MockUserService();

            // Create an instance of the ListGroupController class
            _controller = new ListGroupController(_dbContext, _userService);
        }

        [Fact]
        public void Get_ReturnsListOfGroups()
        {
            // Arrange
            var groups = new List<Group>()
            {
                new Group { groupId = 1, Name = "Group 1" },
                new Group { groupId = 2, Name = "Group 2" },
                new Group { groupId = 3, Name = "Group 3" }
            };
            _dbContext.Groups.AddRange(groups);
            _dbContext.Groups.Count();
            _dbContext.SaveChanges();
         

            // Set up the mock IUserService object to return true for IsAuthenticated method
            ((MockUserService)_userService).IsAuthenticatedResult = true;

            // Act
            var result = _controller.Get();
            Debug.WriteLine(result);
            
            // Assert
            Assert.IsType<ActionResult<List<Group>>>(result);            
            var data = Assert.IsAssignableFrom<List<Group>>(result.Value);
            Assert.Equal(3, data.Count());
        }

       
    }
}
