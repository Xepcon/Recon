using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Account;

namespace Recon.Models.Model.GroupLib
{
    public class GroupService : IGroupService
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        public GroupService(DataDbContext dbContext, IUserService userService) {
            _dbContext = dbContext;
            _userService = userService;

        }

        public List<IPerson> GetGroupMembers(int groupId)
        {
            if (_userService.IsAuthenticated())
            {

                List<IPerson> people = new List<IPerson>();
                var groupMembers = _dbContext.GroupMembers.Where(x => x.groupId == groupId);
                foreach (var member in groupMembers)
                {
                    var person = _dbContext.Person.Where(p => p.userId == member.userId).FirstOrDefault();
                    if (person != null)
                    {
                        people.Add(person);
                    }
                }
                return people;

            }
            return null;

        }

        public bool IsInGroup(int userid, int groupId){
            return _dbContext.GroupMembers.Where(x => x.userId == userid && x.groupId == groupId).Any() ;
        }
        public bool IsInGroup() {
            return _dbContext.GroupMembers.Where(x=>x.userId==_userService.GetUserId()).Any();
        }
        public List<IGroup> getUserGroup()
        {
            List<IGroup> userGroups = new List<IGroup>();

            if (_userService.IsAuthenticated())
            {
                int userId = _userService.GetUserId();
                var userMemberList = _dbContext.GroupMembers.Where(x => x.userId == userId).ToList();

                foreach (var member in userMemberList)
                {
                    var group = _dbContext.Groups.FirstOrDefault(x => x.groupId == member.groupId);
                    if (group != null)
                    {
                        userGroups.Add(new Group { Name = group.Name, groupId = group.groupId, principalId = group.principalId });
                    }
                }
            }

            return userGroups;
        }

        public bool IsGroupOwner(int groupId)
        {
            if(_userService.IsAuthenticated()) {
                int userid = _userService.GetUserId();
                return !_dbContext.Groups.Where(x => x.groupId == groupId && x.principalId == userid).IsNullOrEmpty();
            }
            return false;
        }

        public bool IsInGroup(int groupId)
        {
            if (_userService.IsAuthenticated()) {
                int userid = _userService.GetUserId();
                return !_dbContext.GroupMembers.Where(x => x.groupId==groupId & x.userId == userid).IsNullOrEmpty();
            }
            return false;
        }

        public bool IsGroupOwner()
        {
            if (_userService.IsAuthenticated())
            {
                int userid = _userService.GetUserId();
                var res = _dbContext.Groups.Where(x=>x.principalId== userid);
                return !res.IsNullOrEmpty();
            }
            return false;
        }

        public IEnumerable<Group> GetAllGroups()
        {
            return _dbContext.Groups;
        }

        public Group GetGroupById(int id)
        {
            return _dbContext.Groups.Find(id);
        }

        public void AddGroup(Group group)
        {
            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();
        }

        public void UpdateGroup(Group group)
        {
            _dbContext.Groups.Update(group);
            _dbContext.SaveChanges();
        }

        public void DeleteGroup(int id)
        {
            var model = _dbContext.Groups.Find(id);
            if (model != null)
            {
                _dbContext.Groups.Remove(model);
                _dbContext.SaveChanges();
            }
        }

        public void CreateGroup(Group group) {

            _dbContext.Add(group);
            var member = new GroupMember();


            _dbContext.SaveChanges();
            member.groupId = group.groupId;
            member.userId = group.principalId;
            _dbContext.GroupMembers.Add(member);
            _dbContext.SaveChanges();
        }

        public IEnumerable<GroupMember> GetAllMembers()
        {
            return _dbContext.GroupMembers;
        }

        public GroupMember GetMembersById(int groupid, int userid)
        {
            return _dbContext.GroupMembers.Where(x => x.groupId == groupid && x.userId == userid).FirstOrDefault();
        }

        public bool AddMembers(GroupMember model)
        {
            //Check if user intern only one group allowed 
            if (_userService.GetRolesForUser(model.userId).Where(x => x.Name == "Intern").Any())
            {
                return false;
            }

            var list = _dbContext.GroupMembers.Where(x => x.groupId == model.groupId && x.userId == model.userId).ToList();

            if (list.IsNullOrEmpty())
            {
                _dbContext.Add(model);
                _dbContext.SaveChanges();

                return true;
            }
            return false;
        }

       

        public void DeleteMembers(int groupid, int userid)
        {
            var model = _dbContext.GroupMembers.Where(x => x.groupId == groupid & x.userId == userid).FirstOrDefault();
            if (model != null)
            {
                _dbContext.GroupMembers.Remove(model);
                _dbContext.SaveChanges();
            }
        }
        public List<int> GetGroupIdByPrincipalId(int principalid) {
            var result =  _dbContext.Groups.Where(x=>x.principalId == principalid).ToList();
            if (result != null)
            {
                List<int> groupIds = result.Select(x => x.groupId).ToList();
                return groupIds;
            }
            else
            {
                return new List<int>();
            }
        }
    }
}
