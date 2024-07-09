using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models;

namespace WebApplication58.ViewModels
{
    public class KoszykViewModel
    {
        public int IloscProduktow { get; set; }
        public double Suma { get; set; }
        public List <Koszyk> Koszyki { get; set; }
    }
}
