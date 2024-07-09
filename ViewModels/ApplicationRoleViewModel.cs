using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.ViewModels
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public string Result { get; set; }

    }
}
