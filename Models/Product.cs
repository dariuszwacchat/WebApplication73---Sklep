using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.Models
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int Quantity { get; set; }
        public Dostepnosc Dostepnosc { get; set; }

        public int IloscOdwiedzin { get; set; }

        [DataType(DataType.PhoneNumber)]
        public double Znizka { get; set; }
        public string Rozmiar { get; set; }
        public string Kolor { get; set; }
        public string DataDodania { get; set; }


        public string MarkaId { get; set; }
        public Marka Marka { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }
          
        public string SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }

        public string SubsubcategoryId { get; set; } 
        public Subsubcategory Subsubcategory { get; set; }

        public List<PhotoProduct> PhotosProduct { get; set; }
        public List<ColorProduct> ColorsProduct { get; set; }
        public List<SizeProduct> SizesProduct { get; set; }

        public List <OrderItem> OrderItems { get; set; }
    }
}
