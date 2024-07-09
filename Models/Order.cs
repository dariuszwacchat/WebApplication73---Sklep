using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.Models
{
    public class Order
    {
        public Order ()
        { 
            OrderItems = new List<OrderItem> ();
        }

        [Key]
        public string OrderId { get; set; }
        public int Ilosc { get; set; }
        public double Suma { get; set; }
        public DateTime DataZamowienia { get; set; }
        public DateTime DataRealizacji { get; set; }
        public string SposobPlatnosci { get; set; }
        public string SposobWysylki { get; set; }
        public bool Confirmed { get; set; }
        public StatusZamowienia StatusZamowienia { get; set; }



        public string OsobaRealizujacaId { get; set; }
        public ApplicationUser OsobaRealizujaca { get; set; }


        public string ClientId { get; set; }
        public Client Client { get; set; }

        public List <OrderItem> OrderItems { get; set; }

    }
}
