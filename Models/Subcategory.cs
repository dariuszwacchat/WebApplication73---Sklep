using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.Models
{
    public class Subcategory
    {
        [Key]
        public string SubcategoryId { get; set; }

        //[Required]
        //[DataType(DataType.Text), MinLength(1)]
        public string Name { get; set; }

        //[Required]
        //[DataType(DataType.Text), MinLength(1)]
        public string FullName { get; set; } 

        public int IloscOdwiedzin { get; set; }
        public int Kolejnosc { get; set; }
        public SelectedCategory SubcategoryCategory { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }



#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public List <Subsubcategory>? Subsubcategories { get; set; }

    }
}
