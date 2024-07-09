using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class PhotoProduct
    {
        [Key]
        public string PhotoProductId { get; set; }
        public byte [] PhotoData { get; set; }



        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
