using Recon.Models.Interface.Account;
using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.Account
{
    public class Person : IPerson
    {
        [Key]
        public int userId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool? Phone { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public int? age { get; set; }

        public string? Gender { get; set; }

        public string? Title { get; set; }

        public int? GroupId { get; set; }
    }
}
