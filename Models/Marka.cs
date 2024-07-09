using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class Marka
    {
        [Key]
        public string MarkaId { get; set; }
        public string Name { get; set; }


        public List <Product> Produkty { get; set; }
    }
}
