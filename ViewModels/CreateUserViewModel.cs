using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.ViewModels
{
    public class CreateUserViewModel : RegisterViewModel
    {
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
