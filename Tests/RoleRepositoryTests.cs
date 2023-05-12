using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models.Model.Account;
using Recon.Models.Repository;

namespace Recon.Tests.Repository
{
    public class RoleRepositoryTests
    {
        private readonly DataDbContext _dbContext;
        private readonly RolesRepository _repository;

        public RoleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                          .UseInMemoryDatabase(databaseName: "RoleDb")
                          .Options;

            _dbContext = new DataDbContext(options);
            _dbContext.Database.EnsureCreated();


            if (!_dbContext.Role.Any())
            {
                _dbContext.Role.Add(new Roles { Name = "Hr", Id = 1 });
                _dbContext.Role.Add(new Roles { Name = "Admin", Id = 2 });
                _dbContext.Role.Add(new Roles { Name = "LEVEL 3 SUPPORT", Id = 3 });
                _dbContext.SaveChanges();
            }
            _dbContext.SaveChanges();
            _repository = new RolesRepository(_dbContext);
            
        }

        //[Fact]

        /*public void RoleRepositoryTest()
        {
            Roles fst = new Roles { Name = "Hr", Id = 1 };
            Roles snd = new Roles { Name = "Admin", Id = 2 };
            Roles third = new Roles { Name = "LEVEL 3 SUPPORT", Id = 3 };
            _repository.AddRole(fst);
            _repository.AddRole(snd);
            _repository.AddRole(third);

            IEnumerable<Roles> res = _repository.GetAllRoles();
            Assert.Equal(3, res.Count());
            Roles model = _repository.GetRoleById(1);
            Assert.Equal(1, model.Id);
            Assert.Equal("Hr", model.Name);

            _repository.DeleteRole(1);
            res = _repository.GetAllRoles();
            Assert.Equal(2, res.Count());
            snd.Name = "Test";
            _repository.UpdateRole(snd);
            model = _repository.GetRoleById(2);
            Assert.Equal(2, model.Id);
            Assert.Equal("Test", model.Name);
        }*/
        [Fact]
        public void Unit_AddRoleTest()
        {
            // Arrange
            Roles role = new Roles { Name = "Test Role", Id = 4 };

            // Act
            _repository.AddRole(role);
            _dbContext.SaveChanges(); // Save changes to the in-memory database

            // Assert
            Assert.Equal(4, _repository.GetAllRoles().Count());
            Roles addedRole = _repository.GetRoleById(4);
            Assert.Equal(4, addedRole.Id);
            Assert.Equal("Test Role", addedRole.Name);
        }

        [Fact]
        public void Unit_DeleteRoleTest()
        {
            // Arrange
            int roleId = 1;

            // Act
            _repository.DeleteRole(roleId);
            _dbContext.SaveChanges(); // Save changes to the in-memory database

            // Assert
            Assert.Equal(3, _repository.GetAllRoles().Count());
            Roles deletedRole = _repository.GetRoleById(roleId);
            Assert.Null(deletedRole);
        }

        [Fact]
        public void Unit_GetAllRolesTest()
        {
            // Act
            var roles = _repository.GetAllRoles();

            // Assert
            Assert.Equal(3, roles.Count());
           /* Assert.Contains(roles, r => r.Id == 1 && r.Name == "Hr");
            Assert.Contains(roles, r => r.Id == 2 && r.Name == "Updated Role");
            Assert.Contains(roles, r => r.Id == 3 && r.Name == "LEVEL 3 SUPPORT");*/
        }

        [Fact]
        public void Unit_UpdateRoleTest()
        {
            // Arrange
            int roleId = 2;
            Roles role = _repository.GetRoleById(roleId);
            role.Name = "Updated Role";

            // Act
            _repository.UpdateRole(role);
            _dbContext.SaveChanges(); // Save changes to the in-memory database

            // Assert
            Roles updatedRole = _repository.GetRoleById(roleId);
            Assert.Equal("Updated Role", updatedRole.Name);
        }

        // Dispose of the in-memory database after each test
       public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }


    }
}
