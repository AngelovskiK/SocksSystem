using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BossSystem.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        
        public List<Deposit> Deposits { get; set; }
        public List<Buy> Buys { get; set; }
        public List<Sell> Sells { get; set; }
    }
}
