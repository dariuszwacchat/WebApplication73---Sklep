using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.Models
{
    public class Client
    {
        [Key]
        public string ClientId { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Photo { get; set; }
        public string Telefon { get; set; }
        public string Ulica { get; set; }
        public string Miejscowosc { get; set; }
        public string Kraj { get; set; }
        public string KodPocztowy { get; set; }
        public string DataUrodzenia { get; set; }
        public Plec Plec { get; set; }
        public bool Newsletter { get; set; }
        public string Email { get; set; }
        public RodzajOsoby RodzajOsoby { get; set; }
        public bool HasAccount { get; set; }
        public DateTime DataDodania { get; set; }



        public List <ApplicationUser>? Users { get; set; }
        public List<Favourite>? Favourites { get; set; } 
        public List<Order>? Orders { get; set; }
    }
}
