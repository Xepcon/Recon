using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;

using Recon.Models.Model.GroupLib;

namespace Recon.Models.Interface.GroupLib
{
    public interface IGroupService
    {
        List<IPerson> GetGroupMembers(int groupId);
        bool IsGroupOwner(int groupId);
        bool IsInGroup(int groupId);
        List<IGroup> getUserGroup();
        bool IsGroupOwner();
        bool IsInGroup();
        bool IsInGroup(int userid,int groupId);
        IEnumerable<Group> GetAllGroups();
        Group GetGroupById(int id);
        void AddGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(int id);
        void CreateGroup(Group group);
        IEnumerable<GroupMember> GetAllMembers();
        GroupMember GetMembersById(int groupid, int userid);
        bool AddMembers(GroupMember model);        
        void DeleteMembers(int groupid, int userid);
        List<int> GetGroupIdByPrincipalId(int principalid);

        bool IsGroupOwnerAndMember();
    }
}
