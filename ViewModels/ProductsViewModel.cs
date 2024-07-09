using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models;

namespace WebApplication58.ViewModels
{
    public class ProductsViewModel : BaseViewModel <Product>
    {
        public string ProductId { get; set; }
        public List<Product> Products { get; set; }  

        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string SubsubcategoryName { get; set; }
         

        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }

        public List<string> Kolory { get; set; }
        public List<string> Rozmiary { get; set; }
        public List<string> Marki { get; set; }


        public List<string> SelectedColors { get; set; } = new List<string> ();
        public List<string> SelectedBrands { get; set; } = new List<string>();
        public List<string> SelectedRozmiary { get; set; } = new List<string>(); 
        public List<string> SelectedFilters { get; set; } = new List<string>();
        public string SelectedColorsString { get; set; }
        public string SelectedBrandsString { get; set; }
        public string SelectedRozmiaryString { get; set; }
        public string SelectedFiltersString { get; set; }

        public string SelectedFiltr { get; set; }

        public string Test { get; set; }



        public IFormFile File { get; set; }
    }
}
