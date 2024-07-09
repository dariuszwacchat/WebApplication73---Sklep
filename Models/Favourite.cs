using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class Favourite
    {
        public string FavouriteId { get; set; }

        public string ClientId { get; set; }
        public Client Client { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
