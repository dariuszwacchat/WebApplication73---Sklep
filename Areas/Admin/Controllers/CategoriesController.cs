using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context; 
        public CategoriesController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index (CategoriesViewModel model)
        {
            var categories = await _context.Categories
                .Include (i=> i.Subcategories)
                .OrderBy (o=> o.Name)
                .ToListAsync();

            return View(new CategoriesViewModel()
            {
                Paginator = Paginator<Category>.CreateAsync(categories, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, CategoriesViewModel model)
        {
            List<Category> categories = await _context.Categories
                .Include (i=> i.Subcategories)
                .OrderBy (o=> o.Name)
                .ToListAsync();


            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                categories = categories.Where(w => w.Name.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    categories = categories.OrderBy(o => o.Name).ToList();
                    break;

                case "Nazwa Z-A":
                    categories = categories.OrderByDescending(o => o.Name).ToList();
                    break; 
            }


            return View(new CategoriesViewModel()
            {
                Paginator = Paginator<Category>.CreateAsync(categories, model.PageIndex, model.PageSize),
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Category model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Categories.FirstOrDefaultAsync(f => f.Name == model.Name) == null)
                { 
                    model.CategoryId = Guid.NewGuid().ToString();
                    model.IloscOdwiedzin = 0;

                    _context.Categories.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    ViewData["Error"] = "Wskazana nazwa jest już zajęta. Spóbuj podać inną.";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var category = await _context.Categories
                .Include (i=> i.Subcategories)
                .FirstOrDefaultAsync (f=> f.CategoryId == id);

            if (category == null)
                return NotFound(); 
 
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, Category model)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _context.Categories
                        .Include (i=> i.Subcategories)
                        .FirstOrDefaultAsync (f=> f.CategoryId == id);

                    if (category == null)
                        return NotFound();


                    if (await _context.Categories.FirstOrDefaultAsync(f => f.Name == model.Name) == null)
                    {
                        category.Name = model.Name;
                        category.FullName = model.FullName;

                        _context.Entry(category).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        ViewData["Error"] = "Wskazana nazwa jest już zajęta. Spóbuj podać inną.";
                    } 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Categories.FirstOrDefaultAsync(f => f.CategoryId == id) == null) 
                        return NotFound(); 
                    else 
                        throw; 
                } 
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var category = await _context.Categories
                .Include (i=> i.Subcategories)
                .FirstOrDefaultAsync (f=> f.CategoryId == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var category = await _context.Categories
                .Include (i=> i.Subcategories)
                .FirstOrDefaultAsync (f=> f.CategoryId == id);
             
            if (category != null)
            {
                var subcategories = await _context.Subcategories
                    .Where (w=> w.CategoryId == id)
                    .ToListAsync ();

                var subsubcategories = await _context.Subsubcategories
                    .Where (w=> w.CategoryId == id)
                    .ToListAsync ();

                var products = await _context.Products
                    .Where (w=> w.CategoryId == id)
                    .ToListAsync ();

                foreach (var product in products)
                {
                    _context.Products.Remove (product);
                    await _context.SaveChangesAsync ();
                }

                foreach (var subsubcategory in subsubcategories)
                {
                    _context.Subsubcategories.Remove(subsubcategory);
                    await _context.SaveChangesAsync();
                }

                foreach (var subcategory in subcategories)
                {
                    _context.Subcategories.Remove(subcategory);
                    await _context.SaveChangesAsync();
                }


                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            } 

            return RedirectToAction("Index", "Categories");
        }

        /*
        [HttpGet] 
        public async Task<IActionResult> Subcategories (string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return NotFound();

            var subcategories = await _context.Subcategories
                .Include (i=> i.Category) 
                .Where (w=> w.CategoryId == categoryId)
                .ToListAsync ();

            if (subcategories == null)
                return NotFound(); 

            return View (new SubcategoriesViewModel()
            {
                CategoryId = categoryId,
                Subcategories = subcategories
            });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subcategories (SubcategoriesViewModel model)
        {
            if (string.IsNullOrEmpty(model.CategoryId))
                return NotFound();

            var subcategories = await _context.Subcategories
                .Include (i=> i.Category) 
                .Where (w=> w.CategoryId == model.CategoryId)
                .ToListAsync ();

            if (string.IsNullOrEmpty (model.SearchPhrase))
            { 
                subcategories = Sortowanie (subcategories, model.SortowanieOption).Result;
                return View (new SubcategoriesViewModel ()
                {
                    Subcategories = subcategories,
                    SearchPhrase = model.SearchPhrase,
                    SortowanieOption = model.SortowanieOption,
                    CategoryId = model.CategoryId
                });
            }
            else
            {
                subcategories = subcategories.Where (w=> w.Name.Contains (model.SearchPhrase) ||
                    w.Name.Contains (model.SearchPhrase.ToUpper ()))
                    .ToList ();

                subcategories = Sortowanie(subcategories, model.SortowanieOption).Result;
                return View(new SubcategoriesViewModel()
                {
                    Subcategories = subcategories,
                    SearchPhrase = model.SearchPhrase,
                    SortowanieOption = model.SortowanieOption,
                    CategoryId = model.CategoryId
                });
            }
             
        }*/





    }
}
