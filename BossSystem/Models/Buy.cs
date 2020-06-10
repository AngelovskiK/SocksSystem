using System;
using System.ComponentModel.DataAnnotations;

namespace BossSystem.Models
{
    public class Buy
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int Ammount { get; set; }
        public int Price { get; set; }
        public DateTime TimestampBought { get; set; }    
    }
}
