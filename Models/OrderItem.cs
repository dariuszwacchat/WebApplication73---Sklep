using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class OrderItem
    {
        [Key]
        public string OrderItemId { get; set; }
        public int Ilosc { get; set; }
        public double Suma { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }
         
    }
}
