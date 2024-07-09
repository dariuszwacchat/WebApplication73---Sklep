using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public int IloscOdwiedzin { get; set; }
        public int Kolejnosc { get; set; }


#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public List <Subcategory>? Subcategories { get; set; }

    }
}
