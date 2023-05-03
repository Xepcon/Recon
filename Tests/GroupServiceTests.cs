using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Recon.Models.Model.GroupLib;
using Tests.Mock.Service.User;

namespace Tests
{
    public class GroupServiceTests
    {
        private readonly GroupService _groupService;
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        public GroupServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                          .UseInMemoryDatabase(databaseName: "GroupDb")
                          .Options;

            _dbContext = new DataDbContext(options);


            _userService = new MockUserService();
            
            _groupService = new GroupService(_dbContext, _userService);

        }

        [Fact]
        public void GroupServiceIntegrationTests()
        {

            _dbContext.Person.Add(new Person { userId = 1, FirstName = "Gibsz", LastName = "Jakab" });
            _dbContext.SaveChanges();
            Group FirstGroup = new Group { groupId = 1, Name = "Group 1", principalId = 1 };
            Group SecondGroup = new Group { groupId = 2, Name = "Group 2" };
            Group ThirdGroup = new Group { groupId = 3, Name = "Group 3" };

            GroupMember Memeber1 = new GroupMember { userId = 3, groupId = 1 };
            GroupMember Memeber2 = new GroupMember { userId = 5, groupId = 1 };

            _groupService.AddMembers(Memeber1);
            _groupService.AddMembers(Memeber2);
            _groupService.CreateGroup(FirstGroup);
            _groupService.AddGroup(SecondGroup);
            _groupService.AddGroup(ThirdGroup);

            var list = _groupService.GetAllGroups();
            int result = list.Count();
            Assert.Equal(3, result);

            SecondGroup.principalId = 3;
            _groupService.UpdateGroup(SecondGroup);

            Group group = _groupService.GetGroupById(2);
            Assert.Equal("Group 2", group.Name);
            Assert.Equal(2, group.groupId);
            Assert.Equal(3, group.principalId);

            ((MockUserService)_userService).UserIDResult = 1;
            ((MockUserService)_userService).GetRolesForUserResult = new List<Roles> { new Roles { Id = 1, Name = "Hr" }, new Roles { Id = 2, Name = "Intern" } }; ;
            var isGroupOwnerResult = _groupService.IsGroupOwner();
            Assert.True(isGroupOwnerResult);
            _groupService.DeleteGroup(2);
            list = _groupService.GetAllGroups();
            result = list.Count();
            Assert.Equal(2, result);
            var listGroups = _groupService.GetGroupIdByPrincipalId(1);
            Assert.Equal(1, listGroups.Count());
            var GroupMembersCount = _groupService.GetAllMembers().Count();

            Assert.Equal(3, GroupMembersCount);
            _groupService.DeleteMembers(1, 5);
            GroupMembersCount = _groupService.GetAllMembers().Count();
            List<Roles> rolesList = new List<Roles>();
          
            Assert.Equal(2, GroupMembersCount);

            GroupMember MemberTmp = _groupService.GetMembersById(1, 3);
            Assert.Equal(1, MemberTmp.groupId);
            Assert.Equal(3, MemberTmp.userId);

            var res = _groupService.IsInGroup();
            Assert.True(res);
            res = _groupService.IsInGroup(3, 3);
            Assert.False(res);
            var UserGroups = _groupService.getUserGroup();
            Assert.Equal(1, UserGroups.Count());
            var GroupOwner = _groupService.IsGroupOwner(1);
            Assert.True(GroupOwner);
            GroupOwner = _groupService.IsGroupOwner(2);
            Assert.False(GroupOwner);
            var isInGroupRes = _groupService.IsInGroup(1);
            Assert.True(isInGroupRes);
            isInGroupRes = _groupService.IsInGroup(3);
            Assert.False(isInGroupRes);
            var GroupMembers = _groupService.GetGroupMembers(1);
            Assert.Equal(1, GroupMembers.Count());

        }

        [Fact]
        public void IsGroupOwner_UsingGroupService()
        {
            ((MockUserService)_userService).IsAuthenticatedResult = true;
            ((MockUserService)_userService).UserIDResult = 478;
            var result = _groupService.IsGroupOwner();
            Assert.False(result);

        }



    }

}
