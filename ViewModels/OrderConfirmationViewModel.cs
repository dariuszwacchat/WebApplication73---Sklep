using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models;

namespace WebApplication58.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public Client Client { get; set; }
        public Order Order { get; set; }
        public List <KoszykItem> KoszykItems { get; set; }
    }
}
