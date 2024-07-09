using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

namespace WebApplication58.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager <ApplicationUser> _userManager;
        private readonly UserService _common;

        public OrdersController (ApplicationDbContext context, UserService common, KoszykService koszykService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _common = common;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index (OrdersViewModel model)
        {
            var orders = await _context.Orders
                .Include(i => i.OrderItems)
                .Include(i => i.Client)
                .Include(i => i.OsobaRealizujaca)
                    .ThenInclude (t=> t.Client)
                .Where (w=> w.StatusZamowienia == StatusZamowienia.Niezrealizowane)
                .ToListAsync();


            return View(new OrdersViewModel()
            {
                Orders = orders,
                Paginator = Paginator<Order>.CreateAsync(orders, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, OrdersViewModel model)
        {
            var orders = await _context.Orders
                .Include(i => i.OrderItems)
                .Include(i => i.Client)
                .Include(i => i.OsobaRealizujaca)
                    .ThenInclude (t=> t.Client)
                .ToListAsync();


            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                orders = orders.Where(w => w.Client.Nazwisko.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Zrealizowane":
                    orders = orders.Where(w=> w.StatusZamowienia == StatusZamowienia.Zrealizowane).ToList();
                    break;

                case "Niezrealizowane":
                    orders = orders.Where(w => w.StatusZamowienia == StatusZamowienia.Niezrealizowane).ToList();
                    break;

                case "Wszystkie":
                    break;
            }
             

            return View(new OrdersViewModel()
            {
                Orders = orders,
                Paginator = Paginator<Order>.CreateAsync(orders, model.PageIndex, model.PageSize),
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                Start = model.Start,
                End = model.End,
                q = model.q,
                SortowanieOption = model.SortowanieOption
            });
        }
         


        [HttpGet]
        public async Task<IActionResult> Edit (string orderId)
        {
            var order = await _context.Orders
                .Include(i=> i.Client)
                .Include(i=> i.OrderItems)
                    .ThenInclude(t=> t.Product)
                .FirstOrDefaultAsync (f=> f.OrderId == orderId);

            if (order == null)
                return NotFound();  

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (Order model)
        {  
            if (ModelState.IsValid)
            {
                try
                {
                    Order order = await _context.Orders.FirstOrDefaultAsync (f=> f.OrderId == model.OrderId);
                    if (order != null)
                    {
                        var osobaRealizujaca = await _userManager.GetUserAsync(User);

                        order.StatusZamowienia = model.StatusZamowienia;
                        order.OsobaRealizujacaId = osobaRealizujaca.Id; 
                        order.DataRealizacji = DateTime.Now;
                        _context.Entry(order).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Orders.FirstOrDefaultAsync(f => f.OrderId == model.OrderId) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Orders");
            } 
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> OrderDetails (string orderId)
        {
            var order = await _context.Orders
                .Include(i=> i.OrderItems)
                .FirstOrDefaultAsync (f=> f.OrderId == orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

           

        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            var order = await _context.Orders 
                .FirstOrDefaultAsync (f=> f.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            try
            {
                var order = await _context.Orders
                        .FirstOrDefaultAsync (f=> f.OrderId == id);

                if (order == null)
                    return NotFound();
                 

                if (order != null)
                {
                    // Delete order items 
                    var orderItems = await _context.OrderItems.Where (w=> w.OrderId == order.OrderId).ToListAsync();
                    foreach (var orderItem in orderItems)
                    {
                        _context.OrderItems.Remove(orderItem);
                        await _context.SaveChangesAsync();
                    }
                     

                    // Delete order
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }
            }
            catch { }

            return RedirectToAction("Index", "Orders");
        }



    }
}
