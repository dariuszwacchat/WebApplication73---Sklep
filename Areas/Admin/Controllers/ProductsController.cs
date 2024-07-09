using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;
using WebApplication58.Services;
using WebApplication58.ViewModels;

namespace WebApplication58.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HtmlSanitizationService _htmlSanitizationService;
        public ProductsController (ApplicationDbContext context, HtmlSanitizationService htmlSanitizationService)
        {
            _context = context;
            _htmlSanitizationService = htmlSanitizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index (ProductsViewModel model)
        {
            var products = await _context.Products
                .Include (i=> i.Category)
                .Include (i=> i.Subcategory)
                .Include (i=> i.Subsubcategory)
                .Include (i=> i.ColorsProduct)
                .Include (i=> i.PhotosProduct)
                .Include (i=> i.SizesProduct)
                .Include (i=> i.Marka)
                .OrderBy (o=> o.DataDodania)
                .ToListAsync();
             

            return View(new ProductsViewModel()
            { 
                Paginator = Paginator<Product>.CreateAsync(products, model.PageIndex, model.PageSize),
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                Start = model.Start,
                End = model.End,
                q = model.q, 
                SortowanieOption = model.SortowanieOption
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index (string s, ProductsViewModel model)
        {
            var products = await _context.Products
                .Include (i=> i.Category)
                .Include (i=> i.Subcategory)
                .Include (i=> i.Subsubcategory)
                .Include (i=> i.ColorsProduct)
                .Include (i=> i.PhotosProduct)
                .Include (i=> i.SizesProduct)
                .Include (i=> i.Marka)
                .OrderBy (o=> o.Name)
                .Take(50)
                .ToListAsync();


            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                products = products.Where(w => w.Name.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    products = products.OrderBy(o=> o.Name).ToList();
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

                case "Odwiedziny rosnąco":
                    products = products.OrderBy(o => o.IloscOdwiedzin).ToList();
                    break;

                case "Odwiedziny malejąco":
                    products = products.OrderByDescending(o => o.IloscOdwiedzin).ToList();
                    break;
            } 

            return View(new ProductsViewModel()
            {
                Paginator = Paginator<Product>.CreateAsync(products, model.PageIndex, model.PageSize),
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                Start = model.Start,
                End = model.End,
                q = model.q,
                SortowanieOption = model.SortowanieOption
            });
        }



        [HttpGet]
        public async Task<IActionResult> Create ()
        {
            ViewData["markiIdList"] = new SelectList(await _context.Marki.ToListAsync(), "MarkaId", "Name");
            ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName");
            ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.OrderBy(o => o.Kolejnosc).ToListAsync(), "SubcategoryId", "FullName");
            ViewData["subsubcategoriesIdList"] = new SelectList(await _context.Subsubcategories.ToListAsync(), "SubsubcategoryId", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (List <IFormFile> files, Product model)
        {
            if (ModelState.IsValid)
            {
                var subsubcategory = await _context.Subsubcategories.FirstOrDefaultAsync (f=> f.SubsubcategoryId == model.SubsubcategoryId);

                //string sanitizedHtml = _htmlSanitizationService.SanitizeHtml(model.Description);

                model.ProductId = Guid.NewGuid().ToString();
                //model.Description = sanitizedHtml;
                model.IloscOdwiedzin = 0;
                model.DataDodania = DateTime.Now.ToString();
                model.CategoryId = subsubcategory.CategoryId;
                model.SubcategoryId = subsubcategory.SubcategoryId;
                model.SubsubcategoryId = subsubcategory.SubsubcategoryId;

                _context.Products.Add(model);
                await _context.SaveChangesAsync();


                // Dodanie zdjęcia  
                await CreateNewPhoto(files, model.ProductId);

                return RedirectToAction("Index", "Products");
            }

            ViewData["markiIdList"] = new SelectList(await _context.Marki.ToListAsync(), "MarkaId", "Name", model.MarkaId);
            ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName", model.CategoryId);
            ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.OrderBy(o => o.Kolejnosc).ToListAsync(), "SubcategoryId", "FullName", model.SubcategoryId);
            ViewData["subsubcategoriesIdList"] = new SelectList(await _context.Subsubcategories.ToListAsync(), "SubsubcategoryId", "FullName", model.SubsubcategoryId);

            return View (model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var product = await _context.Products
                .Include (i=> i.Category)
                .Include (i=> i.Subcategory)
                .Include (i=> i.Subsubcategory)
                .Include (i=> i.PhotosProduct)
                .Include (i=> i.Marka)
                .OrderBy (o=> o.Name)
                .FirstOrDefaultAsync(f=> f.ProductId == id);

            if (product == null)
                return NotFound();

            ViewData["markiIdList"] = new SelectList(await _context.Marki.ToListAsync(), "MarkaId", "Name", product.ProductId);
            ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName", product.CategoryId);
            ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.OrderBy(o => o.Kolejnosc).ToListAsync(), "SubcategoryId", "FullName", product.SubcategoryId);
            ViewData["subsubcategoriesIdList"] = new SelectList(await _context.Subsubcategories.OrderBy(o => o.Kolejnosc).ToListAsync(), "SubsubcategoryId", "FullName", product.SubsubcategoryId);


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, List <IFormFile> files, Product model)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products
                        .Include (i=> i.Category)
                        .Include (i=> i.Subcategory)
                        .Include (i=> i.Subsubcategory)
                        .Include (i=> i.Marka)
                        .FirstOrDefaultAsync (f=> f.ProductId == id);

                    if (product == null)
                        return NotFound();

                    product.Name = model.Name;
                    product.Description = model.Description; 
                    product.Price = model.Price;
                    product.Quantity = model.Quantity;
                    product.IloscOdwiedzin = model.IloscOdwiedzin;
                    product.Znizka = model.Znizka;
                    product.MarkaId = model.MarkaId;
                    product.CategoryId = model.CategoryId;
                    product.SubcategoryId = model.SubcategoryId;
                    product.SubsubcategoryId = model.SubsubcategoryId; 

                    _context.Entry(product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();



                    // Dodanie zdjęcia  
                    await CreateNewPhoto(files, model.ProductId);


                    ViewData["markiIdList"] = new SelectList(await _context.Marki.ToListAsync(), "MarkaId", "Name", product.ProductId);
                    ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName", product.CategoryId);
                    ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.OrderBy(o => o.Kolejnosc).ToListAsync(), "SubcategoryId", "FullName", product.SubcategoryId);
                    ViewData["subsubcategoriesIdList"] = new SelectList(await _context.Subsubcategories.OrderBy(o => o.Kolejnosc).ToListAsync(), "SubsubcategoryId", "FullName", product.SubsubcategoryId);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Products.FirstOrDefaultAsync(f => f.SubsubcategoryId == id) == null) 
                        return NotFound(); 
                    else 
                        throw; 
                }
                return RedirectToAction("Index", "Products");
            }
           
            return View(model); 
        }
        

        [HttpGet]
        public async Task <IActionResult> DeletePhoto (string productId, string photoProductId)
        {
            var photoProduct = await _context.PhotosProduct.FirstOrDefaultAsync (f=> f.ProductId == productId && f.PhotoProductId == photoProductId);
            if (photoProduct == null)
                return NotFound();

            _context.PhotosProduct.Remove(photoProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Products", new { id = productId });
            //return RedirectToAction("Index", "Products");
        }




        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var product = await _context.Products
                        .FirstOrDefaultAsync (f=> f.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return NotFound();

                var product = await _context.Products
                        .FirstOrDefaultAsync (f=> f.ProductId == id);

                if (product != null)
                {
                    // Delete photos 
                    var photos = await _context.PhotosProduct.Where (w=> w.ProductId == product.ProductId).ToListAsync();
                    foreach (var photo in photos)
                    {
                        _context.PhotosProduct.Remove(photo);
                        await _context.SaveChangesAsync();
                    }

                    // Delete colors 
                    var colors = await _context.ColorsProduct.Where (w=> w.ProductId == product.ProductId).ToListAsync();
                    foreach (var color in colors)
                    {
                        _context.ColorsProduct.Remove(color);
                        await _context.SaveChangesAsync();
                    }

                    // Delete sizes
                    var rozmiary = await _context.SizesProduct.Where (w=> w.ProductId == product.ProductId).ToListAsync();
                    foreach (var rozmiar in rozmiary)
                    {
                        _context.SizesProduct.Remove(rozmiar);
                        await _context.SaveChangesAsync();
                    }


                    // Delete product
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch { } 

            return RedirectToAction("Index", "Products");
        }




        private async Task CreateNewPhoto (List<IFormFile> files, string productId)
        {
            try
            {
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            byte [] photoData;
                            using (var stream = new MemoryStream())
                            {
                                file.CopyTo(stream);
                                photoData = stream.ToArray();
                            }

                            PhotoProduct photoProduct = new PhotoProduct ()
                            {
                                PhotoProductId = Guid.NewGuid ().ToString (),
                                PhotoData = photoData,
                                ProductId = productId
                            };
                            _context.PhotosProduct.Add(photoProduct);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch { }
        }




    }
}
