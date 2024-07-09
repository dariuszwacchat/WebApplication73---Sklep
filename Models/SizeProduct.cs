using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class SizeProduct
    {
        [Key]
        public string SizeProductId { get; set; }
        public string Name { get; set; }



        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
