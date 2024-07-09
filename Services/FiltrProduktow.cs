using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;

namespace WebApplication58.Services
{
    public class FiltrProduktow
    {
        private readonly ApplicationDbContext _context;
        private List <Product> _products;

        public FiltrProduktow (ApplicationDbContext context)
        {
            _context = context;
            _products = new List<Product> ();
            _products = _context.Products.ToList ();
        }

        public List <List<ColorProduct>> GetColorsProduct ()
        {
            return _context.Products.Select (s=> s.ColorsProduct).ToList();
        }


    }
}
