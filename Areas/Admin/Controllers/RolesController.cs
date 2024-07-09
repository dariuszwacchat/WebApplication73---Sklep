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
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RolesController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index (RolesViewModel model)
        {
            var roles = await _context.Roles.ToListAsync();

            return View(new RolesViewModel()
            {
                Paginator = Paginator<ApplicationRole>.CreateAsync(roles, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, RolesViewModel model)
        {
            var roles = await _context.Roles.ToListAsync();

            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                roles = roles.Where(w => w.Name.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    roles = roles.OrderBy(o => o.Name).ToList();
                    break;

                case "Nazwa Z-A":
                    roles = roles.OrderByDescending(o => o.Name).ToList();
                    break;
            }


            return View(new RolesViewModel()
            {
                Paginator = Paginator<ApplicationRole>.CreateAsync(roles, model.PageIndex, model.PageSize),
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
            => View(new ApplicationRoleViewModel() { Result = "" });


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (ApplicationRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if ((await _context.Roles.FirstOrDefaultAsync(f=>f.Name == model.Name)) == null)
                    {
                        ApplicationRole role = new ApplicationRole ()
                        {
                            Id = Guid.NewGuid ().ToString (),
                            Name = model.Name,
                            NormalizedName = model.Name,
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        };

                        _context.Roles.Add(role);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Roles");
                    }
                    else
                    {
                        model.Result = "Podana nazwa jest już zajęta.";
                    }

                }
                catch { }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit (string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                return NotFound();

            var role = await _context.Roles.FirstOrDefaultAsync (f=> f.Id == roleId);

            if (role == null)
                return NotFound();

            return View(new ApplicationRoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (ApplicationRoleViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _context.Roles.FirstOrDefaultAsync (f=> f.Id == model.Id); 

                    if (role == null)
                        return NotFound();

                    if (role.Name != model.Name)
                    {
                        role.Name = model.Name;

                        _context.Entry(role).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        model.Result = "Podana nazwa jest już zajęta.";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Roles.FirstOrDefaultAsync(f => f.Id == model.Id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Roles");
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var role = await _context.Roles.FirstOrDefaultAsync (f=> f.Id == id);

            if (role == null)
                return NotFound();

            return View(role);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var role = await _context.Roles.FirstOrDefaultAsync (f=> f.Id == id);

            if (role == null)
                return NotFound();

            if (role != null)
            {

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Roles");
        }




    }
}
