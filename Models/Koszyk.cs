using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class Koszyk
    {
        [Key]
        public string KoszykId { get; set; }
        public int Ilosc { get; set; }
        public double Cena { get; set; }

        public string ClientId { get; set; }
        public Client Client { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
