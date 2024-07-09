using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Models.Enum;

namespace WebApplication58.Models
{
    public class NavigationInfo
    {
        public string CategoryId { get; set; }
        public string SubcategoryId { get; set; }
        public string SubsubcategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string SubsubcategoryName { get; set; }
        public Navigation Navigation { get; set; }
    }
}
