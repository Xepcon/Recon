namespace Recon.Models.Interface.Ticket
{
    public interface ITicket
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int userId { get; set; }

        public int groupId { get; set; }


    }
}
