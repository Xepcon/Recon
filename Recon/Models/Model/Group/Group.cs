using Recon.Models.Interface.Group;
using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.Group
{
    public class Group : IGroup
    {
        public string Name { get; set; }

        [Key]
        public int groupId { get; set; }

        public int principalId { get; set; }

    }
}
