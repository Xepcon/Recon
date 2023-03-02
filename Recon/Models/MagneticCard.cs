﻿using System.ComponentModel.DataAnnotations;

namespace Recon.Models
{
    public class MagneticCard : IMagneticCard
    {
        
        public string CardId { get; set; }
       
        public string UserId { get; set; }

        public string CardName { get; set; }
        public string CardType { get; set; }
    }
}
