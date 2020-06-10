using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BossSystem.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int MoneyBalance { get; set; }
        public int SocksBalance { get; set; }
    }
}
