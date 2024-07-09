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
    public class RozmiaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RozmiaryController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index (RozmiaryViewModel model)
        {
            var rozmiary = await _context.Sizes.ToListAsync();

            return View(new RozmiaryViewModel()
            {
                Paginator = Paginator<Size>.CreateAsync(rozmiary, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, RozmiaryViewModel model)
        {
            var rozmiary = await _context.Sizes.ToListAsync();

            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                rozmiary = rozmiary.Where(w => w.Name.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();

            

            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Rozmiar rosnąco":
                    rozmiary = rozmiary.OrderBy(o => o.Name).ToList();
                    break;

                case "Rozmiar malejąco":
                    rozmiary = rozmiary.OrderByDescending(o => o.Name).ToList();
                    break; 
            }

            return View(new RozmiaryViewModel()
            {
                Paginator = Paginator<Size>.CreateAsync(rozmiary, model.PageIndex, model.PageSize),
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
            => View(new RozmiarViewModel() { Result = "" });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (RozmiarViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Zapobiega dodaniu drugi raz tej samej nazwy
                if (await _context.Sizes.FirstOrDefaultAsync(f => f.Name == model.Name.ToString()) == null)
                {

                    Size size = new Size ()
                    {
                        SizeId = Guid.NewGuid ().ToString (),
                        Name = model.Name.ToString(),
                    };

                    _context.Sizes.Add(size);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Rozmiary");
                }
                else
                {
                    model.Result = "Podana nazwa już istnieje w bazie.";
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Edit (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var rozmiar = await _context.Sizes.FirstOrDefaultAsync (f => f.SizeId == id);

            if (rozmiar == null)
                return NotFound();

            return View(rozmiar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, Size model)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var rozmiar = await _context.Sizes.FirstOrDefaultAsync (f => f.SizeId == id);

                    if (rozmiar == null)
                        return NotFound();

                    rozmiar.Name = model.Name;

                    _context.Entry(rozmiar).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Sizes.FirstOrDefaultAsync(f => f.SizeId == id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Rozmiary");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var rozmiar = await _context.Sizes.FirstOrDefaultAsync (f => f.SizeId == id);

            if (rozmiar == null)
                return NotFound();

            return View(rozmiar);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var rozmiar = await _context.Sizes.FirstOrDefaultAsync (f => f.SizeId == id);

            if (rozmiar != null)
            {
                _context.Sizes.Remove(rozmiar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Rozmiary");
        }

    }
}
