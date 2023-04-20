using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models.Model.Account;
using Recon.Models.Repository;

namespace Recon.Tests.Repository
{
    public class UsersInRolesRepositoryTests
    {
        private readonly DataDbContext _dbContext;
        private readonly UsersInRolesRepository _repository;

        public UsersInRolesRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                          .UseInMemoryDatabase(databaseName: "GroupDb")
                          .Options;

            _dbContext = new DataDbContext(options);


            _repository = new UsersInRolesRepository(_dbContext);

        }

        [Fact]
        public void IntegrationTestUserInRoleRepository()
        {

            UsersInRoles fst = new UsersInRoles { roleId = 1, userId = 1 };
            UsersInRoles snd = new UsersInRoles { roleId = 2, userId = 1 };
            UsersInRoles thi = new UsersInRoles { roleId = 2, userId = 2 };
            _repository.Add(fst);
            _repository.Add(snd);
            _repository.Add(thi);



            var result = _repository.GetAll().ToList();


            Assert.Equal(3, result.Count);
            _repository.Delete(1, 1);
            result = _repository.GetAll().ToList();


            Assert.Equal(2, result.Count);


            var model = _repository.GetById(2, 2);
            Assert.Equal(2, model.roleId);
            Assert.Equal(2, model.userId);
        }






    }
}