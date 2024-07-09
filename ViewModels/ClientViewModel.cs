using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.ViewModels
{
    public class ClientViewModel
    {
        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } 

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text), MinLength(2)]
        public string Imie { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text), MinLength(3)]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "*")]
        public string Ulica { get; set; }

        [Required(ErrorMessage = "*")]
        public string KodPocztowy { get; set; }

        [Required(ErrorMessage = "*")]
        public string Miejscowosc { get; set; }

        [Required(ErrorMessage = "*")]
        public string Kraj { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.PhoneNumber)]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "*")]
        public RodzajOsoby RodzajOsoby { get; set; }

        public bool Newsletter { get; set; }


         
    }
}
