using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int IloscZalogowan { get; set; }
        public string DataOstatniegoZalogowania { get; set; }
        public DateTime DataDodania { get; set; }

          

        public string ClientId { get; set; }
        public Client Client { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
