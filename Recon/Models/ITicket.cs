namespace Recon.Models
{
    public interface ITicket
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string UserID { get; set; }


    }
}
