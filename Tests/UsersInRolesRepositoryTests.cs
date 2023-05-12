using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
            _dbContext.Database.EnsureCreated();


            if (!_dbContext.UsersInRole.Any())
            {
                _dbContext.UsersInRole.Add(new UsersInRoles { roleId = 1, userId = 1 });
                _dbContext.UsersInRole.Add(new UsersInRoles { roleId = 2, userId = 1 });
                _dbContext.UsersInRole.Add(new UsersInRoles { roleId = 2, userId = 2 });
                _dbContext.SaveChanges();
            }
            _dbContext.SaveChanges();

            _repository = new UsersInRolesRepository(_dbContext);

        }

        /*[Fact]
        public void IntegrationTestUserInRoleRepository()
        {

            /*UsersInRoles fst = ;
            UsersInRoles snd = ;
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
        }*/

        [Fact]
        public void Unit_AddUserInRoleRepository()
        {
            UsersInRoles model = new UsersInRoles { roleId = 3, userId = 3 };
            _repository.Add(model);

            var result = _repository.GetAll().ToList();


            Assert.Equal(3, result.Count);
        }


        [Fact]

        public void Unit_DeleteUsersInRoleRepository()
        {
            _repository.Delete(1, 1);
            var result = _repository.GetAll().ToList();

            Assert.Equal(2, result.Count);

        }

        [Fact]

        public void Unit_GetByIdUsersInRoleRepository()
        {
            var model = _repository.GetById(2, 2);
            Assert.Equal(2, model.roleId);
            Assert.Equal(2, model.userId);

        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

    }
}