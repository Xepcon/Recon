using Recon.Models.Interface.Account;

using Recon.Models.Model.GroupLib;

namespace Recon.Models.Interface.GroupLib
{
    public interface IGroupService
    {
        /// Get list of Group  
        List<IPerson> GetGroupMembers(int groupId);
        // Check if user group owner of given id 
        bool IsGroupOwner(int groupId);
        /// Check if user is in Group of given id 
        bool IsInGroup(int groupId);
        // list users groups
        List<IGroup> getUserGroup();
        // check if user is group owner generally
        bool IsGroupOwner();
        // check if the user in group generally
        bool IsInGroup();
        // check if the given user is in group of given group
        bool IsInGroup(int userid, int groupId);
        /// get all Groups
        IEnumerable<Group> GetAllGroups();
        // get the group by id 
        Group GetGroupById(int id);
        // add group to db
        void AddGroup(Group group);
        // Update group in db 
        void UpdateGroup(Group group);
        // delete group in db 
        void DeleteGroup(int id);
        // create group and add principal to group memebers
        void CreateGroup(Group group);
        // get all Members
        IEnumerable<GroupMember> GetAllMembers();
        /// get Member by group id and user id 
        GroupMember GetMembersById(int groupid, int userid);
        // add a group member to db 
        bool AddMembers(GroupMember model);
        // delete a group member from db 
        void DeleteMembers(int groupid, int userid);
        // get a group by principal id 
        List<int> GetGroupIdByPrincipalId(int principalid);
        // Return true if Group owner of the group and only member of this group
        bool IsGroupOwnerAndMember();
    }
}
