using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.GroupLib
{
    public class GroupMember
    {
        [Key]
        public int groupId { get; set; }

        [Key]
        public int userId { get; set; }
    }
}
