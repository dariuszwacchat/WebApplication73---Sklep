using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;
using WebApplication58.Models.Enum;
using WebApplication58.Services;
using WebApplication58.ViewModels;

namespace WebApplication58.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; 
        public HomeController (ApplicationDbContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public async Task<IActionResult> Index ()
        {

            /*Info.NavigationInfo = new NavigationInfo() { Navigation = Navigation.Index };
            var products = await _context.Products.ToListAsync ();
            return View(new HomeViewModel()
            {
                Products = products,
                CenaDo = 1000,
            }); */
            //return RedirectToAction("Index", "Products");
            return Redirect ("https://localhost:44390/Admin/Home");
            //return View ();
        }



        [HttpGet]
        public IActionResult Test ()
        { 
            return View ();
        }









        /*
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Index (HomeViewModel model)
                { 
                    *//*model.Products = (await _context.Products.ToListAsync()).Take(40).ToList();

                    if (model.MinPrice > 0 && model.MaxPrice > 0)
                        model.Products = await _context.Products
                            .Where(w => w.Price >= model.MinPrice && w.Price <= model.MaxPrice)
                            .ToListAsync();*//*



                    //return View(model);

                    return RedirectToAction ("Index", "Products");
                }



                private Task<List<Product>> SelectMarka (List<Product> source, List<string> marki)
                {
                    List <Product> products = new List<Product> ();
                    foreach (string marka in marki)
                        products = source.Where(s => s.Marka == marka).ToList();
                    return
                        Task.FromResult(products);
                } 
                private Task<List<Product>> SelectRozmiar (List<Product> source, List<string> rozmiary)
                {
                    List <Product> products = new List<Product> ();
                    foreach (string rozmiar in rozmiary)
                        products = source.Where(s => s.Rozmiar == rozmiar).ToList();
                    return
                        Task.FromResult(products);
                }
                private Task<List<Product>> SelectColor (List<Product> source, List<string> kolory)
                {
                    List <Product> products = new List<Product> ();
                    foreach (string kolor in kolory)
                        products = source.Where(s => s.Kolor == kolor).ToList();
                    return
                        Task.FromResult (products);
                }


                // usuwa powtarzające się elementy w tablicy
                private Task<List<Product>> Clear (List<Product> source, HomeViewModel model)
                {
                    List <Product> products = new List<Product> ();
                    foreach (var s in source.Where (w=> w.Price >= model.CenaOd && w.Price <= model.CenaDo).ToList())
                        if (!products.Contains(s))
                            products.Add(s);
                    return
                        Task.FromResult(products);
                }


                private async Task<HomeViewModel> Wyszukiwarka (HomeViewModel model)
                {
                    //List <Product> allProducts = await _context.Products.ToListAsync ();
                    List <Product> allProducts = model.Products; 
                    List <Product> products = new List<Product> (); 



                    List <string> selectedMark = new List<string> ();
                    if (model.Luxilon)
                        selectedMark.Add("Luxilon"); 
                    if (model.Wilson)
                        selectedMark.Add("Wilson"); 
                    if (model.Babolat)
                        selectedMark.Add("Babolat"); 
                    if (model.Volkl)
                        selectedMark.Add("Volkl"); 
                    if (model.Head)
                        selectedMark.Add("Head"); 
                    if (model.Yonex)
                        selectedMark.Add("Yonex"); 
                    if (model.Lacoste)
                        selectedMark.Add("Lacoste"); 
                    if (model.Tencifibre)
                        selectedMark.Add("Tencifibre");


                    List <string> selectedRozmiar = new List<string> ();
                    if (model.R33a)
                        selectedRozmiar.Add("33");
                    if (model.R33b)
                        selectedRozmiar.Add("33.5");
                    if (model.R34a)
                        selectedRozmiar.Add("34");
                    if (model.R34b)
                        selectedRozmiar.Add("34.5");
                    if (model.R35a)
                        selectedRozmiar.Add("35");
                    if (model.R35b)
                        selectedRozmiar.Add("35.5");


                    List <string> selectedColors = new List<string> ();
                    if (model.Bialy)
                        selectedColors.Add("Biały");
                    if (model.Niebieski)
                        selectedColors.Add("Niebieski");
                    if (model.Zielony)
                        selectedColors.Add("Zielony");
                    if (model.Czarny)
                        selectedColors.Add("Czarny");


                    List <Product> markaProducts = new List<Product> ();
                    List <Product> rozmiarProducts = new List<Product> ();
                    List <Product> kolorProducts = new List<Product> ();


                    foreach (var p in allProducts)
                    {
                        foreach (string marka in selectedMark)
                        {
                            if (p.Marka == marka)
                            {
                                if (!markaProducts.Contains(p))
                                    markaProducts.Add(p);
                            }
                        }
                    }


                    foreach (var p in allProducts)
                    {
                        foreach (string rozmiar in selectedRozmiar)
                        {
                            if (p.Rozmiar == rozmiar)
                            {
                                if (!rozmiarProducts.Contains(p))
                                    rozmiarProducts.Add(p);
                            }
                        }
                    }

                    foreach (var p in allProducts)
                    {
                        foreach (string kolor in selectedColors)
                        {
                            if (p.Kolor == kolor)
                            {
                                if (!kolorProducts.Contains(p))
                                    kolorProducts.Add(p);
                            }
                        }
                    }


                    *//*if (selectedMark.Count > 0)
                         markaProducts = SelectMarka(markaProducts, selectedMark).Result;*//*
                    if (selectedRozmiar.Count > 0)
                        markaProducts = SelectRozmiar(markaProducts, selectedRozmiar).Result;
                    if (selectedColors.Count > 0)
                        markaProducts = SelectColor(markaProducts, selectedColors).Result;


                    if (selectedMark.Count > 0)
                        rozmiarProducts = SelectMarka(rozmiarProducts, selectedMark).Result;
                    *//*if (selectedRozmiar.Count > 0)
                         rozmiarProducts = SelectRozmiar(rozmiarProducts, selectedRozmiar).Result;*//*
                    if (selectedColors.Count > 0)
                        rozmiarProducts = SelectColor(rozmiarProducts, selectedColors).Result;


                    if (selectedMark.Count > 0)
                        kolorProducts = SelectMarka(kolorProducts, selectedMark).Result;
                    if (selectedRozmiar.Count > 0)
                        kolorProducts = SelectRozmiar(kolorProducts, selectedRozmiar).Result;
                    *//*if (selectedColors.Count > 0)
                         kolorProducts = SelectColor(kolorProducts, selectedColors).Result;*//*




                    products.AddRange(markaProducts);
                    products.AddRange(rozmiarProducts);
                    products.AddRange(kolorProducts);

                    products = Clear(products, model).Result;

                    // Sortowanie
                    // ------------------------------------------------------------------------------

                   *//* switch (model.SortowanieString)
                    {
                        case "A":
                            products = products.OrderBy(o => o.DataDodania).ToList();
                            break;
                        case "B":
                            products = products.OrderByDescending(o => o.DataDodania).ToList();
                            break;
                        case "C":
                            products = products.OrderByDescending(o => o.Name).ToList();
                            break;
                        case "D":
                            products = products.OrderBy(o => o.Price).ToList();
                            break;
                        case "E":
                            products = products.OrderByDescending(o => o.Price).ToList();
                            break;
                    } *//*

                    // ------------------------------------------------------------------------------



                    *//*
                                if (selectedMark.Count == 0 && selectedRozmiar.Count == 0 && selectedColors.Count == 0)
                                    products = allProducts;*//*

                    return new HomeViewModel()
                    {
                        Products = products,
                        ILOSC = $"{markaProducts.Count},{rozmiarProducts.Count},{kolorProducts.Count}",
                        Marka = $"{selectedMark.Count}, {selectedRozmiar.Count}, {selectedColors.Count}",
                        *//*Babolat = model.Babolat,
                        CenaDo = model.CenaDo,
                        CenaOd = model.CenaOd,
                        Bialy = model.Bialy,
                        Czarny = model.Czarny,
                        Niebieski = model.Niebieski,
                        Zielony = model.Zielony,
                        Head = model.Head,
                        Lacoste = model.Lacoste,
                        Luxilon = model.Luxilon,
                        R33a = model.R33a,
                        R33b = model.R33b,
                        R34a = model.R34a,
                        R34b = model.R34b,
                        R35a = model.R35a,
                        R35b = model.R35b,
                        Volkl = model.Volkl,
                        Wilson = model.Wilson,
                        Yonex = model.Yonex,
                        Tencifibre = model.Tencifibre*//*
                        SortowanieString = model.SortowanieString
                    };
                }



                [HttpGet]
                public async Task<IActionResult> CategoryDetails (HomeViewModel model)
                {
                    var products = await _context.Products
                        .Include (i=> i.Category)
                        .Include (i=> i.Subcategory)
                        .Include (i=> i.Subsubcategory)
                        .Where (f=> f.CategoryId == model.CategoryId)
                        .ToListAsync ();

                    if (products == null)
                        return NotFound();


                    var category = await _context.Categories.FirstOrDefaultAsync(f=> f.CategoryId == model.CategoryId);

                    *//*category.IloscOdwiedzin = category.IloscOdwiedzin + 1;
                    _context.Entry(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();*//*

                    Info.NavigationInfo = new NavigationInfo()
                    {
                        Navigation = Navigation.Category,
                        CategoryId = category.CategoryId,
                        CategoryName = category.FullName
                    };

                    return View(new HomeViewModel()
                    {
                        Products = products, 
                        CenaDo = 1000
                    });
                }


                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> CategoryDetails (string s, HomeViewModel model)
                {
                    model.Products = await _context.Products
                        .Include (i=> i.Category)
                        .Include (i=> i.Subcategory)
                        .Include (i=> i.Subsubcategory)
                        .Where (f=> f.CategoryId == model.CategoryId)
                        .ToListAsync (); 

                    return View(await Wyszukiwarka(model));
                }


                [HttpGet]
                public async Task<IActionResult> SubcategoryDetails (HomeViewModel model)
                {
                    var products = await _context.Products
                        .Include (i=> i.Category)
                        .Include (i=> i.Subcategory)
                        .Include (i=> i.Subsubcategory)
                        .Where (f=> f.CategoryId == model.CategoryId && f.SubcategoryId == model.SubcategoryId)
                        .ToListAsync ();

                    if (products == null)
                        return NotFound();

                    var category = await _context.Categories.FirstOrDefaultAsync(f=> f.CategoryId == model.CategoryId);
                    var subcategory = await _context.Subcategories.FirstOrDefaultAsync (f=>f.SubcategoryId == model.SubcategoryId);

                    //category.IloscOdwiedzin = category.IloscOdwiedzin + 1;
                    //subcategory.IloscOdwiedzin = subcategory.IloscOdwiedzin + 1;

                    //_context.Entry(category).State = EntityState.Modified;
                   // _context.Entry(subcategory).State = EntityState.Modified;
                    //await _context.SaveChangesAsync();

                    Info.NavigationInfo = new NavigationInfo()
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.FullName,
                        SubcategoryId = subcategory.SubcategoryId,
                        SubcategoryName = subcategory.FullName,
                        Navigation = Navigation.Subcategory
                    }; 

                    return View(new HomeViewModel()
                    {
                        Products = products,
                        CenaDo = 1000
                    });
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> SubcategoryDetails (string s, HomeViewModel model)
                {
                    model.Products = await _context.Products
                        .Include(i => i.Category)
                        .Include(i => i.Subcategory)
                        .Include(i => i.Subsubcategory)
                        .Where(f => f.CategoryId == model.CategoryId && f.SubcategoryId == model.SubcategoryId)
                        .ToListAsync();

                    var products = await Wyszukiwarka (model);

                    if (products == null)
                        return NotFound();

                    return View(await Wyszukiwarka(model));
                }



                [HttpGet]
                public async Task<IActionResult> SubsubcategoryDetails (HomeViewModel model)
                {
                    var products = await _context.Products
                        .Include (i=> i.Category)
                        .Include (i=> i.Subcategory)
                        .Include (i=> i.Subsubcategory)
                        .Where (f=> f.CategoryId == model.CategoryId && f.SubcategoryId == model.SubcategoryId && f.SubsubcategoryId == model.SubsubcategoryId)
                        .ToListAsync ();

                    if (products == null)
                        return NotFound();

                    var category = await _context.Categories.FirstOrDefaultAsync(f=> f.CategoryId == model.CategoryId);
                    var subcategory = await _context.Subcategories.FirstOrDefaultAsync (f=>f.SubcategoryId == model.SubcategoryId);
                    var subsubcategory = await _context.Subsubcategories.FirstOrDefaultAsync (f=> f.SubsubcategoryId == model.SubsubcategoryId);

                    category.IloscOdwiedzin = category.IloscOdwiedzin + 1;
                    subcategory.IloscOdwiedzin = subcategory.IloscOdwiedzin + 1;
                    subsubcategory.IloscOdwiedzin = subsubcategory.IloscOdwiedzin + 1;

                    *//*_context.Entry(category).State = EntityState.Modified;
                    _context.Entry(subcategory).State = EntityState.Modified;
                    _context.Entry(subsubcategory).State = EntityState.Modified;
                    await _context.SaveChangesAsync();*//*

                    Info.NavigationInfo = new NavigationInfo()
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.FullName,
                        SubcategoryId = subcategory.SubcategoryId,
                        SubcategoryName = subcategory.FullName,
                        SubsubcategoryId = subsubcategory.SubsubcategoryId,
                        SubsubcategoryName = subsubcategory.FullName,
                        Navigation = Navigation.Subsubcategory
                    };

                    return View(new HomeViewModel()
                    {
                        Products = products,
                        CenaDo = 1000
                    });
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> SubsubcategoryDetails (string s, HomeViewModel model)
                {
                    model.Products = await _context.Products
                        .Include(i => i.Category)
                        .Include(i => i.Subcategory)
                        .Include(i => i.Subsubcategory)
                        .Where(f => f.CategoryId == model.CategoryId && f.SubcategoryId == model.SubcategoryId)
                        .ToListAsync();

                    var products = await Wyszukiwarka (model);

                    if (products == null)
                        return NotFound();

                    return View(await Wyszukiwarka(model));
                }



                public async Task<IActionResult> ProductDetails (string productId)
                {
                    var product = await _context.Products
                        .Include (i=> i.Category)
                        .Include (i=> i.Subcategory)
                        .Include (i=> i.Subsubcategory)
                        .FirstOrDefaultAsync (f=> f.ProductId == productId);
                    if (product == null)
                        return NotFound ();

                    product.IloscOdwiedzin = product.IloscOdwiedzin + 1;
                    _context.Entry (product).State = EntityState.Modified;
                    await _context.SaveChangesAsync ();

                    Info.NavigationInfo = new NavigationInfo()
                    {
                        Navigation = Navigation.ProductDetails,
                        CategoryId = product.Category.CategoryId,
                        CategoryName = product.Category.Name,
                        SubcategoryId = product.Subcategory.SubcategoryId,
                        SubcategoryName = product.Subcategory.Name,
                        SubsubcategoryId = product.Subsubcategory.SubsubcategoryId,
                        SubsubcategoryName = product.Subsubcategory.Name, 
                    };

                    return View(product);
                }
        */




        [HttpGet]
        public IActionResult Success ()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Cancel ()
        {
            return View();
        }




    }


}
