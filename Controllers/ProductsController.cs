using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;
using WebApplication58.Services;
using WebApplication58.ViewModels;

namespace WebApplication58.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly KoszykService _koszykService;

        public ProductsController (ApplicationDbContext context, KoszykService koszykService)
        {
            _context = context;
            _koszykService = koszykService;
        }

        [HttpGet]
        public async Task<IActionResult> Index (ProductsViewModel model)
        {
            var products = await _context.Products
                .Include(i=> i.PhotosProduct)
                .Include(i=> i.ColorsProduct)
                .Include(i=> i.SizesProduct)
                .Include(i=> i.Category)
                .Include(i=> i.Marka)
                .ToListAsync ();
             

            if (!string.IsNullOrEmpty(model.CategoryName))
            {
                products = _context.Products
                 .Include(i => i.Category)
                 .Where(w => w.Category.Name == model.CategoryName)
                 .ToList();
                 
                 
            } 
            if (!string.IsNullOrEmpty(model.SubcategoryName))
            {
                products = _context.Products
                 .Include(i => i.Category)
                 .Include(i => i.Subcategory)
                 .Where(w => w.Category.Name == model.CategoryName &&
                             w.Subcategory.Name == model.SubcategoryName)
                 .ToList();
                 
            } 
            if (!string.IsNullOrEmpty(model.SubsubcategoryName))
            {
                products = _context.Products
                 .Include(i => i.Category)
                 .Include(i => i.Subcategory)
                 .Include(i => i.Subsubcategory)
                 .Where(w => w.Category.Name == model.CategoryName &&
                             w.Subcategory.Name == model.SubcategoryName &&
                             w.Subsubcategory.Name == model.SubsubcategoryName)
                 .ToList();
                 
            }
             

            List <string> kolory = await _context.Colors.Select(s => s.Name).ToListAsync();
            List <string> marki = await _context.Marki.Select(s => s.Name).ToListAsync();
            List <string> rozmiary = await _context.Sizes.Select(s => s.Name).ToListAsync();

            /*
                        List <Product> filtrMarka = new List<Product> ();
                        List <Product> filtrKolor = new List<Product> ();
                        List <Product> filtrRozmiar = new List<Product> ();*/


            /* tutaj trzeba zamienić ze strina na listę i z listy na stringa ponieważ nie można przekazywać
               listy elementów z widoku do kontrolera, dlatego ta zamiana. Dane przekazywane są
               poprzez buttony Prev, środkowe linki oraz Next
            */
            List <string> selectedBrands = new List<string> ();
            List <string> selectedColors = new List<string> ();
            List <string> selectedRozmiary = new List<string> ();
            List <string> selectedFilters = new List<string> ();


            if (!string.IsNullOrEmpty(model.SelectedBrandsString))
                selectedBrands = model.SelectedFiltersString.Split(',').ToList();

            if (!string.IsNullOrEmpty(model.SelectedColorsString))
                selectedColors = model.SelectedFiltersString.Split(',').ToList();

            if (!string.IsNullOrEmpty(model.SelectedRozmiaryString))
                selectedRozmiary = model.SelectedFiltersString.Split(',').ToList();

            if (!string.IsNullOrEmpty(model.SelectedFiltersString))
                selectedFilters = model.SelectedFiltersString.Split(',').ToList();


            /*
             susunięcie wybranych filtrów z list w których występują,
             filtry usuwa użytkownik poprzez kliknięcie w nie
             */

            if (selectedBrands.Contains(model.SelectedFiltr))
                selectedBrands.Remove(model.SelectedFiltr);

            if (selectedColors.Contains(model.SelectedFiltr))
                selectedColors.Remove(model.SelectedFiltr);

            if (selectedRozmiary.Contains(model.SelectedFiltr))
                selectedRozmiary.Remove(model.SelectedFiltr);

            if (selectedFilters.Contains(model.SelectedFiltr))
                selectedFilters.Remove(model.SelectedFiltr);





            // Filtrowanie po markach
            if (selectedBrands.Any()) 
                products = products.Where(w => selectedBrands.Any(a => a == w.Marka.Name)).ToList(); 

            // Filtrowanie po kolorach
            if (selectedColors.Any()) 
                products = products.Where(w => selectedColors.Any(a => a == w.Kolor)).ToList(); 

            // Filtrowanie po rozmiarach
            if (selectedRozmiary.Any()) 
                products = products.Where(w => selectedRozmiary.Any(a => a == w.Rozmiar)).ToList();



            /* Tutaj obsługiwany wyjątek taki, że jeżeli podczas usuwania filtrów przez użytkownika 
               może pojawić się 0 produktów, bo będą usuwane w różnej kolejności, wtedy czyścimy wszystkie
               listy z filtrami i przypisujemy wszystkie produkty z wybranej kategorii
            */
            if (products.Count == 0)
            {
                selectedBrands.Clear();
                selectedColors.Clear();
                selectedRozmiary.Clear();
                selectedFilters.Clear();
                if (!string.IsNullOrEmpty(model.CategoryName))
                {
                    products = _context.Products
                     .Include(i => i.Category)
                     .Where(w => w.Category.Name == model.CategoryName)
                     .ToList(); 
                }
                if (!string.IsNullOrEmpty(model.SubcategoryName))
                {
                    products = _context.Products
                     .Include(i => i.Category)
                     .Include(i => i.Subcategory)
                     .Where(w => w.Category.Name == model.CategoryName &&
                                 w.Subcategory.Name == model.SubcategoryName)
                     .ToList();

                }
                if (!string.IsNullOrEmpty(model.SubsubcategoryName))
                {
                    products = _context.Products
                     .Include(i => i.Category)
                     .Include(i => i.Subcategory)
                     .Include(i => i.Subsubcategory)
                     .Where(w => w.Category.Name == model.CategoryName &&
                                 w.Subcategory.Name == model.SubcategoryName &&
                                 w.Subsubcategory.Name == model.SubsubcategoryName)
                     .ToList();

                }
            }



            /*
                Odczytanie modelu z sesji, jest to potrzebne po to aby nie zerowały się dane po kliknięciu
                na przycisk "DodajDoKoszyka"
             */

            ProductsViewModel sessionModel = HttpContext.Session.GetObject<ProductsViewModel>("ProductsViewModel");
            if (sessionModel != null)
            {
                return View(new ProductsViewModel
                {
                    Products = products,
                    Kolory = kolory,
                    Marki = marki,
                    Rozmiary = rozmiary,

                    CategoryName = sessionModel.CategoryName,
                    SubcategoryName = sessionModel.SubcategoryName,
                    SubsubcategoryName = sessionModel.SubsubcategoryName,


                    // przekazywanie danych od przycisku do akcji kontrolera
                    SelectedBrands = selectedBrands,
                    SelectedBrandsString = sessionModel.SelectedBrandsString,

                    SelectedColors = selectedColors,
                    SelectedColorsString = sessionModel.SelectedColorsString,

                    SelectedRozmiary = selectedRozmiary,
                    SelectedRozmiaryString = sessionModel.SelectedRozmiaryString,

                    SelectedFilters = selectedFilters,
                    SelectedFiltersString = sessionModel.SelectedFiltersString,


                    // Filtr, który jest zaznaczony w poziomej górnej belce
                    SelectedFiltr = sessionModel.SelectedFiltr,

                    Paginator = Paginator<Product>.CreateAsync(products, sessionModel.PageIndex, sessionModel.PageSize),
                    PageIndex = sessionModel.PageIndex,
                    PageSize = sessionModel.PageSize,
                    Start = sessionModel.Start,
                    End = sessionModel.End,
                    q = sessionModel.q,
                    SearchOption = sessionModel.SearchOption,
                    SortowanieOption = sessionModel.SortowanieOption
                });
            }
            else
                return View(new ProductsViewModel
                {
                    Products = products,
                    Kolory = kolory,
                    Marki = marki,
                    Rozmiary = rozmiary, 

                    CategoryName = string.IsNullOrEmpty(model.CategoryName) ? "RakietyTenisowe" : model.CategoryName,
                    SubcategoryName = model.SubcategoryName,
                    SubsubcategoryName = model.SubsubcategoryName,
                     


                    // przekazywanie danych od przycisku do akcji kontrolera
                    SelectedBrands = selectedBrands,
                    SelectedBrandsString = model.SelectedBrandsString,

                    SelectedColors = selectedColors,
                    SelectedColorsString = model.SelectedColorsString,

                    SelectedRozmiary = selectedRozmiary,
                    SelectedRozmiaryString = model.SelectedRozmiaryString,

                    SelectedFilters = selectedFilters,
                    SelectedFiltersString = model.SelectedFiltersString,


                    // Filtr, który jest zaznaczony w poziomej górnej belce
                    SelectedFiltr = model.SelectedFiltr,

                    Paginator = Paginator<Product>.CreateAsync(products, model.PageIndex, model.PageSize),
                    PageIndex = model.PageIndex,
                    PageSize = model.PageSize,
                    Start = model.Start,
                    End = model.End,
                    q = model.q,
                    SearchOption = model.SearchOption,
                    SortowanieOption = model.SortowanieOption
                });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index (string s, ProductsViewModel model)
        {
            var products = await _context.Products
                .Include(i=> i.Category)
                .Include(i=> i.Subcategory)
                .Include(i=> i.Subsubcategory)
                .Include(i=> i.PhotosProduct)
                /*.Include(i=> i.ColorsProduct)
                .Include(i=> i.SizesProduct)*/
                .Include(i=> i.Marka)
                .ToListAsync ();



            model.Kolory = await _context.Colors.Select(s => s.Name).ToListAsync();
            model.Marki = await _context.Marki.Select(s => s.Name).ToListAsync();
            model.Rozmiary = await _context.Sizes.Select(s => s.Name).ToListAsync();
             

            // Odfiltrowanie produktów według wybranej kategorii

            if (!string.IsNullOrEmpty(model.CategoryName))
                products = products.Where(w => w.Category.Name == model.CategoryName).ToList();

            if (!string.IsNullOrEmpty(model.SubcategoryName))
                products = products.Where(w => w.Category.Name == model.CategoryName &&
                             w.Subcategory.Name == model.SubcategoryName).ToList();

            if (!string.IsNullOrEmpty(model.SubsubcategoryName)) 
                products = products.Where(w => w.Category.Name == model.CategoryName &&
                             w.Subcategory.Name == model.SubcategoryName &&
                             w.Subsubcategory.Name == model.SubsubcategoryName).ToList();


            // Filtrowanie po markach
            if (model.SelectedBrands.Any()) 
                products = products.Where(w => model.SelectedBrands.Any(a => a == w.Marka.Name)).ToList(); 

            // Filtrowanie po kolorach
            if (model.SelectedColors.Any()) 
                products = products.Where(w => model.SelectedColors.Any(a => a == w.Kolor)).ToList(); 

            // Filtrowanie po rozmiarach
            if (model.SelectedRozmiary.Any()) 
                products = products.Where(w => model.SelectedRozmiary.Any(a => a == w.Rozmiar)).ToList(); 


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    products = products.OrderBy(o => o.Name).ToList();
                    break;

                case "Nazwa Z-A":
                    products = products.OrderByDescending(o => o.Name).ToList();
                    break;

                case "Cena rosnąco":
                    products = products.OrderBy(o => o.Price).ToList();
                    break;

                case "Cena malejąco":
                    products = products.OrderByDescending(o => o.Price).ToList();
                    break;
            }


            model.Products = products;
            model.SelectedFilters = model.SelectedBrands.Union(model.SelectedColors.Union(model.SelectedRozmiary)).ToList(); 
            model.Paginator = Paginator<Product>.CreateAsync(products, model.PageIndex, model.PageSize);
            return View(model);
        }
         
         



        [HttpGet]
        public async Task<IActionResult> ProductItems (string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return NotFound();

            var products = await _context.Products
                .Where (w=> w.Category.Name == categoryName)
                .ToListAsync ();

            if (products == null)
                return NotFound();

            return View(products);
        }





        [HttpGet]
        public async Task<IActionResult> Details (string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return NotFound();

            var product = await _context.Products
                .Include (i=> i.PhotosProduct)
                .FirstOrDefaultAsync(f=> f.ProductId == productId);

            if (product == null)
                return NotFound();

            return View(product);
        }



        [HttpGet]
        public IActionResult DodajDoKoszyka (ProductsViewModel model)
        {
            var product = _context.Products.FirstOrDefault (f=> f.ProductId == model.ProductId);
            if (product != null)
                _koszykService.Create(product);

            // Dodanie modelu do sesji
            HttpContext.Session.SetObject("ProductsViewModel", model);

            return RedirectToAction("Index", "Products");
        }


    }
}
