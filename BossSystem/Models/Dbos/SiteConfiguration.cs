using System.ComponentModel.DataAnnotations;

namespace BossSystem.Models.Dbos
{
    public class SiteConfiguration
    {
        [Required]
        [Key]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
