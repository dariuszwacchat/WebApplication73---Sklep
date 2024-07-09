using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.ViewModels
{
    public class RozmiarViewModel
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Wprowadź liczbę z kropką lub bez kropki do pola.")]
        public double Name { get; set; }

        public string Result { get; set; }
    }
}
