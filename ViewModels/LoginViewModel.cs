using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress), MinLength(5)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password), MinLength(10)]
        public string Password { get; set; }
        public string LoginResult { get; set; }
    }
}
