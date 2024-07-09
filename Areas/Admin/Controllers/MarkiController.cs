using Microsoft.AspNetCore.Authorization;
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

namespace WebApplication58.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class MarkiController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MarkiController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index (MarkiViewModel model)
        {
            var marki = await _context.Marki.ToListAsync ();

            return View(new MarkiViewModel()
            {
                Paginator = Paginator<Marka>.CreateAsync(marki, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, MarkiViewModel model)
        {
            var marki = await _context.Marki.ToListAsync ();


            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                marki = marki.Where(w => w.Name.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    marki = marki.OrderBy(o => o.Name).ToList();
                    break;

                case "Nazwa Z-A":
                    marki = marki.OrderByDescending(o => o.Name).ToList();
                    break;
            }

            return View(new MarkiViewModel()
            {
                Paginator = Paginator<Marka>.CreateAsync(marki, model.PageIndex, model.PageSize),
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                Start = model.Start,
                End = model.End,
                q = model.q,
                SortowanieOption = model.SortowanieOption
            });
        }


        [HttpGet]
        public IActionResult Create ()
            => View ();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Marka model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Marki.FirstOrDefaultAsync (f=> f.Name == model.Name) == null)
                {
                    model.MarkaId = Guid.NewGuid().ToString();

                    _context.Marki.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Marki");
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

            var marka = await _context.Marki.FirstOrDefaultAsync (f => f.MarkaId == id);

            if (marka == null)
                return NotFound();

            return View(marka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, Marka model)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                { 
                    var marka = await _context.Marki.FirstOrDefaultAsync (f => f.MarkaId == id);

                    if (marka == null)
                        return NotFound();

                    if (await _context.Marki.FirstOrDefaultAsync(f => f.Name == model.Name) == null)
                    { 
                        marka.Name = model.Name; 
                        _context.Entry(marka).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewData["Error"] = "Wskazana nazwa jest już zajęta.";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Marki.FirstOrDefaultAsync(f => f.MarkaId == id) == null) 
                        return NotFound(); 
                    else 
                        throw; 
                }
                return RedirectToAction("Index", "Marki");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var marka = await _context.Marki.FirstOrDefaultAsync (f => f.MarkaId == id);

            if (marka == null)
                return NotFound();

            return View(marka);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var marka = await _context.Marki.FirstOrDefaultAsync (f => f.MarkaId == id);

            if (marka != null)
            { 
                _context.Marki.Remove(marka);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Marki");
        }
         

    }
}
