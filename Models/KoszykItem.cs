using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class KoszykItem
    {
        public string KoszykItemId { get; set; }
        public int Ilosc { get; set; }
        public double Suma { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
