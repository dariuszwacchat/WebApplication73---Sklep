using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models;

namespace WebApplication58.ViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; } = new List<string> (); 
    }
}
