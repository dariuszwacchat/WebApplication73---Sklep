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
    public class SubcategoriesController : Controller
    {
        private readonly ApplicationDbContext _context; 
        public SubcategoriesController (ApplicationDbContext context)
        {
            _context = context; 
        }


        [HttpGet]
        public async Task<IActionResult> Index (SubcategoriesViewModel model)
        {
            var subcategories = await _context.Subcategories
                .Include (i=> i.Subsubcategories)
                .Include (i=> i.Category)
                .OrderBy (o=> o.Name)
                .ToListAsync();


            return View(new SubcategoriesViewModel()
            {
                Paginator = Paginator<Subcategory>.CreateAsync(subcategories, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, SubcategoriesViewModel model)
        {
            var subcategories = await _context.Subcategories
                .Include (i=> i.Subsubcategories)
                .Include (i=> i.Category)
                .OrderBy (o=> o.Name)
                .ToListAsync();

            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                subcategories = subcategories.Where(w => w.Name.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    subcategories = subcategories.OrderBy(o => o.Name).ToList();
                    break;

                case "Nazwa Z-A":
                    subcategories = subcategories.OrderByDescending(o => o.Name).ToList();
                    break;
            }


            return View(new SubcategoriesViewModel()
            {
                Paginator = Paginator<Subcategory>.CreateAsync(subcategories, model.PageIndex, model.PageSize),
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                Start = model.Start,
                End = model.End,
                q = model.q,
                SortowanieOption = model.SortowanieOption
            });
        }


        private Task<List<Subcategory>> Sortowanie (List<Subcategory> subcategories, string sortowanie)
        {
            List <Subcategory> result = new List<Subcategory> ();
            switch (sortowanie)
            {
                case "Nazwa-Rosnąco":
                    result = subcategories.OrderBy(o => o.Name).ToList();
                    break;
                case "Nazwa-Malejąco":
                    result = subcategories.OrderByDescending(o => o.Name).ToList();
                    break;

                case "Odwiedziny-Rosnąco":
                    result = subcategories.OrderBy(o => o.IloscOdwiedzin).ToList();
                    break;
                case "Odwiedziny-Malejąco":
                    result = subcategories.OrderByDescending(o => o.IloscOdwiedzin).ToList();
                    break; 

            }

            return Task.FromResult(result);
        }




        [HttpGet]
        public async Task <IActionResult> Create ()
        {
            ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName");
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Subcategory model)
        {
            if (ModelState.IsValid)
            {  
                if (await _context.Subcategories.FirstOrDefaultAsync (f=> f.Name == model.Name) == null)
                {
                    model.SubcategoryId = Guid.NewGuid().ToString();
                    model.IloscOdwiedzin = 0;

                    _context.Subcategories.Add(model);
                    await _context.SaveChangesAsync();

                    ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName", model.CategoryId); 
                    return RedirectToAction("Create", "Subcategories");
                }
                else
                {
                    ViewData["Error"] = "Wskazana nazwa jest już zajęta.";
                } 
            }
            return View(model);
        }

        public async Task<IActionResult> Edit (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var subcategory = await _context.Subcategories
                .Include (i=> i.Category) 
                .FirstOrDefaultAsync (f=> f.SubcategoryId == id);
             
            if (subcategory == null)
                return NotFound();

            ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName", subcategory.CategoryId);

            return View(subcategory);
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
                    var subcategory = await _context.Subcategories
                        .Include (i=> i.Category) 
                        .FirstOrDefaultAsync (f=> f.SubcategoryId == id);

                    if (subcategory == null)
                        return NotFound();


                    if (await _context.Subcategories.FirstOrDefaultAsync(f => f.Name == model.Name) == null)
                    { 
                        subcategory.Name = model.Name;
                        subcategory.FullName = model.FullName;
                        subcategory.CategoryId = model.CategoryId;

                        _context.Entry(subcategory).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        ViewData["categoriesIdList"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "FullName", subcategory.CategoryId);

                        return RedirectToAction("Index", "Subcategories");
                    }
                    else
                    {
                        ViewData["Error"] = "Wskazana nazwa jest już zajęta.";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Subcategories.FirstOrDefaultAsync(f => f.SubcategoryId == id) == null) 
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

            var subcategory = await _context.Subcategories 
                  .FirstOrDefaultAsync (f=> f.SubcategoryId == id);

            if (subcategory == null)
                return NotFound();

            return View(subcategory);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var subcategory = await _context.Subcategories 
                  .FirstOrDefaultAsync (f=> f.SubcategoryId == id);

            if (subcategory != null)
            { 
                var subsubcategories = await _context.Subsubcategories
                        .Where (w=> w.SubcategoryId == id && w.CategoryId == subcategory.CategoryId)
                        .ToListAsync();

                var products = await _context.Products
                        .Where (w=> w.SubcategoryId == id && w.CategoryId == subcategory.CategoryId)
                        .ToListAsync();

                foreach (var product in products)
                {
                    _context.Products.Remove (product);
                    await _context.SaveChangesAsync ();
                }

                foreach (var subsubcategory in subsubcategories)
                {
                    _context.Subsubcategories.Remove (subsubcategory);
                    await _context.SaveChangesAsync();
                }

                _context.Subcategories.Remove(subcategory);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index", "Subcategories");
        }

         

    }
}
