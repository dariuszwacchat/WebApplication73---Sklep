using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models;

namespace WebApplication58.ViewModels
{
    public class OrdersViewModel : BaseViewModel <Order>
    { 
        public List<Order> Orders { get; set; }
    }
}
