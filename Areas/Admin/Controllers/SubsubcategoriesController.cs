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
    public class SubsubcategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubsubcategoriesController (ApplicationDbContext context)
        {
            _context = context;
        }
         
         
        [HttpGet]
        public async Task<IActionResult> Index (SubsubcategoriesViewModel model)
        {
            var subsubcategories = await _context.Subsubcategories
                .Include (i=> i.Products)
                .Include (i=> i.Category)
                .Include (i=> i.Subcategory)
                .OrderBy (o=> o.Name)
                .ToListAsync();

            return View(new SubsubcategoriesViewModel()
            {
                Paginator = Paginator<Subsubcategory>.CreateAsync(subsubcategories, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, SubsubcategoriesViewModel model)
        {
            var subsubcategories = await _context.Subsubcategories
                .Include (i=> i.Products)
                .Include (i=> i.Category)
                .Include (i=> i.Subcategory)
                .OrderBy (o=> o.Name)
                .ToListAsync();

            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                subsubcategories = subsubcategories.Where(w => w.Name.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    subsubcategories = subsubcategories.OrderBy(o => o.Name).ToList();
                    break;

                case "Nazwa Z-A":
                    subsubcategories = subsubcategories.OrderByDescending(o => o.Name).ToList();
                    break;
            }


            return View(new SubsubcategoriesViewModel()
            {
                Paginator = Paginator<Subsubcategory>.CreateAsync(subsubcategories, model.PageIndex, model.PageSize),
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
            ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.ToListAsync(), "SubcategoryId", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Subsubcategory model)
        {
            if (ModelState.IsValid)
            { 
                if (await _context.Subsubcategories.FirstOrDefaultAsync(f => f.Name == model.Name) == null)
                {
                    var subcategory = await _context.Subcategories.FirstOrDefaultAsync (f=> f.SubcategoryId == model.SubcategoryId);

                    model.SubsubcategoryId = Guid.NewGuid().ToString();
                    model.IloscOdwiedzin = 0;
                    model.CategoryId = subcategory.CategoryId;

                    _context.Subsubcategories.Add(model);
                    await _context.SaveChangesAsync();

                    ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.ToListAsync(), "SubcategoryId", "FullName");

                    return RedirectToAction("Create", "Subsubcategories");
                }
                else
                {
                    ViewData["Error"] = "Wskazana nazwa jest już zajęta.";
                } 
            }
            return View(model);
        }
         
        [HttpGet]
        public async Task<IActionResult> Edit (string id)
        { 
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var subsubcategory = await _context.Subsubcategories
                 .Include (i=> i.Products)
                 .Include (i=> i.Category)
                 .Include (i=> i.Subcategory)
                 .FirstOrDefaultAsync (f=> f.SubcategoryId == id);

            if (subsubcategory == null)
                return NotFound();

            ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.ToListAsync(), "SubcategoryId", "FullName", subsubcategory.SubcategoryId);

            return View(subsubcategory); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, Subsubcategory model)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var subsubcategory = await _context.Subsubcategories 
                        .FirstOrDefaultAsync (f=> f.SubcategoryId == id);

                    if (subsubcategory == null)
                        return NotFound();

                    if (await _context.Subsubcategories.FirstOrDefaultAsync(f => f.Name == model.Name) == null)
                    {
                        subsubcategory.Name = model.Name;
                        subsubcategory.FullName = model.FullName;
                        subsubcategory.CategoryId = model.CategoryId;

                        _context.Entry(subsubcategory).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        ViewData["subcategoriesIdList"] = new SelectList(await _context.Subcategories.ToListAsync(), "SubcategoryId", "FullName", subsubcategory.SubcategoryId);
                    }
                    else
                    {
                        ViewData["Error"] = "Wskazana nazwa jest już zajęta.";
                    } 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Subsubcategories.FirstOrDefaultAsync(f => f.SubsubcategoryId == id) == null) 
                        return NotFound(); 
                    else 
                        throw; 
                }
                return RedirectToAction("Index", "Subsubcategories");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var subsubcategory = await _context.Subsubcategories
                        .FirstOrDefaultAsync (f=> f.SubcategoryId == id);

            if (subsubcategory == null)
                return NotFound();

            return View(subsubcategory);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var subsubcategory = await _context.Subsubcategories
                .Include (i=> i.Products)
                .FirstOrDefaultAsync (f=> f.SubcategoryId == id);

            if (subsubcategory != null)
            { 
                foreach (var product in subsubcategory.Products)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }

                _context.Subsubcategories.Remove(subsubcategory);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index", "Subsubcategories");
        }


        private async Task RemoveAllItems (string subsubcategoryId, string productId)
        {
            var products = await _context.Products
                .Where (w=> w.SubsubcategoryId == subsubcategoryId && w.ProductId == productId)
                .ToListAsync ();

            foreach (var product in products)
            {
                _context.Products.Remove (product);
                await _context.SaveChangesAsync ();
            }
        }


    }
}
