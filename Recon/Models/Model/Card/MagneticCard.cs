using Recon.Models.Interface.Card;
using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.Card
{
    public class MagneticCard : IMagneticCard
    {
        public string CardId { get; set; }

        public int userId { get; set; }

        public string CardName { get; set; }
        public string CardType { get; set; }
    }
}
