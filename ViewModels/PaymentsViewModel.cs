using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models;

namespace WebApplication58.ViewModels
{
    public class PaymentsViewModel
    {
        public string OrderId { get; set; }

        [Required(ErrorMessage = "*")]
        public string SposobPlatnosci { get; set; }

        [Required(ErrorMessage = "*")]
        public string SposobWysylki { get; set; }


    }
}
