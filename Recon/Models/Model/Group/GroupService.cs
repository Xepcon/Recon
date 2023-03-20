using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.Group;
using Recon.Models.Model.Account;
using System.Text.RegularExpressions;

namespace Recon.Models.Model.Group
{
    public class GroupService : IGroupService
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        public GroupService(DataDbContext dbContext, IUserService userService) {
            _dbContext = dbContext;
            _userService = userService;

        }

        public List<IPerson> getGroupMembers(int groupId)
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

        public bool isInGroup(int userid, int groupId){
            return _dbContext.GroupMembers.Where(x => x.userId == userid && x.groupId == groupId).Any() ;
        }
        public bool isInGroup() {
            return _dbContext.GroupMembers.Where(x=>x.userId==_userService.getUserId()).Any();
        }
        public List<IGroup> getUserGroup()
        {
            List<IGroup> userGroups = new List<IGroup>();

            if (_userService.IsAuthenticated())
            {
                int userId = _userService.getUserId();
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

        public bool isGroupOwner(int groupId)
        {
            if(_userService.IsAuthenticated()) {
                int userid = _userService.getUserId();
                return !_dbContext.Groups.Where(x => x.groupId == groupId && x.principalId == userid).IsNullOrEmpty();
            }
            return false;
        }

        public bool isInGroup(int groupId)
        {
            if (_userService.IsAuthenticated()) {
                int userid = _userService.getUserId();
                return !_dbContext.GroupMembers.Where(x => x.groupId==groupId & x.userId == userid).IsNullOrEmpty();
            }
            return false;
        }

        public bool isGroupOwner()
        {
            if (_userService.IsAuthenticated())
            {
                int userid = _userService.getUserId();
                var res = _dbContext.Groups.Where(x=>x.principalId== userid);
                return !res.IsNullOrEmpty();
            }
            return false;
        }
    }
}
