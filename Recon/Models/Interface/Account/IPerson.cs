namespace Recon.Models.Interface.Account
{
    public interface IPerson
    {
        int userId { get; set; }
        string? FirstName { get; }
        string? LastName { get; }

        bool? Phone { get; }

        string? PhoneNumber { get; }

        string? Address { get; }

        string? City { get; }

        string? State { get; }

        string? PostalCode { get; }

        string? Country { get; }

        int? age { get; }
        string? Gender { get; }

        string? Title { get; }

        int? GroupId { get; }

    }
}
