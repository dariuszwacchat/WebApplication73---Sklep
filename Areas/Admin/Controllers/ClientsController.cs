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
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public ClientsController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index (ClientsViewModel model)
        {
            var clients = await _context.Clients
                .Include(i => i.Orders)
                .ToListAsync();

            return View(new ClientsViewModel()
            {
                Paginator = Paginator<Client>.CreateAsync(clients, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, ClientsViewModel model)
        {
            var clients = await _context.Clients
                .Include(i => i.Orders)
                .ToListAsync();


            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                clients = clients.Where(w => w.Nazwisko.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Nazwa A-Z":
                    clients = clients.OrderBy(o => o.Nazwisko).ToList();
                    break;

                case "Nazwa Z-A":
                    clients = clients.OrderByDescending(o => o.Nazwisko).ToList();
                    break;

                case "Zakupione towary rosnąco":
                    clients = clients.OrderBy(o => o.Orders.Sum(s=>s.Suma)).ToList();
                    break;

                case "Zakupione towary malejąco":
                    clients = clients.OrderByDescending(o => o.Orders.Sum(s => s.Suma)).ToList();
                    break; 
            }

            return View(new ClientsViewModel()
            {
                Paginator = Paginator<Client>.CreateAsync(clients, model.PageIndex, model.PageSize),
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
            => View ();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Client client = new Client ()
                    {
                        ClientId = Guid.NewGuid ().ToString (),
                        Imie = model.Imie,
                        Nazwisko = model.Nazwisko,
                        Ulica = model.Ulica,
                        Miejscowosc = model.Miejscowosc,
                        KodPocztowy = model.KodPocztowy,
                        Kraj = model.Kraj,
                        Telefon = model.Telefon, 
                        Email = model.Email,
                        RodzajOsoby = model.RodzajOsoby,
                        Newsletter = model.Newsletter,
                        HasAccount = false,
                        DataDodania = DateTime.Now
                    };
                    _context.Clients.Add (client);
                    await _context.SaveChangesAsync ();
                    return RedirectToAction("Index", "Clients");
                }
                catch { }
            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit (string clientId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync (f=> f.ClientId == clientId);

            if (client == null)
                return NotFound();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (Client model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(model).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Orders.FirstOrDefaultAsync(f => f.OrderId == model.ClientId) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Clients");
            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details (string clientId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync (f=> f.ClientId == clientId);

            if (client == null)
                return NotFound();

            return View(client);
        }



        [HttpGet]
        public async Task<IActionResult> Delete (string clientId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync (f=> f.ClientId == clientId);

            if (client == null)
                return NotFound();

            return View(client);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string clientId)
        {
            try
            {
                var client = await _context.Clients.FirstOrDefaultAsync (f=> f.ClientId == clientId);

                if (client == null)
                    return NotFound(); 


                if (client != null)
                {
                    // Delete user
                    var user = await _context.Users.FirstOrDefaultAsync (w=> w.ClientId == client.ClientId);
                     
                        _context.Users.Remove(user);
                        await _context.SaveChangesAsync();
                     

                    // Delete favourities
                    var favourites = await _context.Favourites.Where (w=> w.ClientId == client.ClientId).ToListAsync();
                    foreach (var favourite in favourites)
                    {
                        _context.Favourites.Remove(favourite);
                        await _context.SaveChangesAsync();
                    }

                    // Delete orders
                    var orders = await _context.Orders.Where (w=> w.ClientId == client.ClientId).ToListAsync();
                    foreach (var order in orders)
                    {
                        _context.Orders.Remove(order);
                        await _context.SaveChangesAsync();
                    }


                    // Delete client
                    _context.Clients.Remove(client);
                    await _context.SaveChangesAsync();
                }
            }
            catch { }

            return RedirectToAction("Index", "Clients");
        }


    }
}
