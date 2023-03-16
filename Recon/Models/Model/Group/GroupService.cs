using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.Group;
using Recon.Models.Model.Account;

namespace Recon.Models.Model.Group
{
    public class GroupService : IGroupService
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        public GroupService(DataDbContext dbContext, IUserService userService) {
            _dbContext= dbContext;
            _userService= userService;

        }

        public List<Person> getGroupMembers(int groupId)
        {
            if (_userService.IsAuthenticated())
            {

                List<Person> people = new List<Person>();
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
    }
}
