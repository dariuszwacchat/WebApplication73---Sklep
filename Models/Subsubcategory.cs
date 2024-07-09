using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class Subsubcategory
    {
        [Key]
        public string SubsubcategoryId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int IloscOdwiedzin { get; set; }
        public int Kolejnosc { get; set; }


        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public string SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }


#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public List <Product>? Products { get; set; }
    }
}
