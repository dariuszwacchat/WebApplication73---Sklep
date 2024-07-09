using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.Models
{
    public class Error
    {
        [Key]
        public string ErrorId { get; set; }
        public string Akcja { get; set; }
        public string Controller { get; set; }
        public RodzajAkcji RodzajAkcji { get; set; }
        public string Komunikat { get; set; }

    }
}
