using Recon.Models.Model.Account;

namespace Recon.Models.Interface.Group
{
    public interface IGroupService
    {
        List<Person> getGroupMembers(int groupId);

        bool isGroupOwner(int groupId);

        bool isInGroup(int groupId);

        
    }
}
