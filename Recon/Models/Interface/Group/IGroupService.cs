using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;

namespace Recon.Models.Interface.Group
{
    public interface IGroupService
    {
        List<IPerson> getGroupMembers(int groupId);

        bool isGroupOwner(int groupId);

        bool isInGroup(int groupId);

        List<IGroup> getUserGroup();

        bool isGroupOwner();
    }
}
