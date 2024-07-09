using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models;
using WebApplication58.Models.Enum;

namespace WebApplication58.ViewModels
{
    public class HomeViewModel : BaseViewModel <Product>
    {
        public string CategoryId { get; set; }
        public string SubcategoryId { get; set; }
        public string SubsubcategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string SubsubcategoryName { get; set; }
        public Product Product { get; set; }
        public List <Product> Products { get; set; }

        public SelectedCategory SelectedCategory { get; set; }

        public double CenaOd { get; set; }
        public double CenaDo { get; set; }
        public string Marka { get; set; }

        public bool Luxilon { get; set; }
        public bool Wilson { get; set; }
        public bool Babolat { get; set; }
        public bool Volkl { get; set; }
        public bool Head { get; set; }
        public bool Yonex { get; set; }
        public bool Lacoste { get; set; }
        public bool Tencifibre { get; set; }



        public bool Bialy { get; set; }
        public bool Niebieski { get; set; }
        public bool Zielony { get; set; }
        public bool Czarny { get; set; }



        public bool R33a { get; set; }
        public bool R33b { get; set; }
        public bool R34a { get; set; }
        public bool R34b { get; set; }
        public bool R35a { get; set; }
        public bool R35b { get; set; }

        public string ILOSC { get; set; } 

        public string SortowanieString { get; set; }
    }
}
