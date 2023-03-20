using Recon.Models.Interface.GroupLib;
using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.GroupLib
{
    public class Group : IGroup
    {
        public string Name { get; set; }

        [Key]
        public int groupId { get; set; }

        public int principalId { get; set; }

    }
}
