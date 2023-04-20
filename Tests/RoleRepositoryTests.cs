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


            _repository = new RolesRepository(_dbContext);

        }

        [Fact]

        public void IntegrationTestForRoleRepository()
        {
            Roles fst = new Roles { Name = "HR", Id = 1 };
            Roles snd = new Roles { Name = "Admin", Id = 2 };
            Roles third = new Roles { Name = "LEVEL 3 SUPPORT", Id = 3 };
            _repository.AddRole(fst);
            _repository.AddRole(snd);
            _repository.AddRole(third);
            IEnumerable<Roles> res = _repository.GetAllRoles();
            Assert.Equal(3, res.Count());
            Roles model = _repository.GetRoleById(1);
            Assert.Equal(1, model.Id);
            Assert.Equal("HR", model.Name);

            _repository.DeleteRole(1);
            res = _repository.GetAllRoles();
            Assert.Equal(2, res.Count());
            snd.Name = "Test";
            _repository.UpdateRole(snd);
            model = _repository.GetRoleById(2);
            Assert.Equal(2, model.Id);
            Assert.Equal("Test", model.Name);
        }
    }
}
