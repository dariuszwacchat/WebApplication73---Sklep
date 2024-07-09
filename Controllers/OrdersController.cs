using Microsoft.AspNetCore.Authorization;
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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _common;
        private readonly KoszykService _koszykService;
        private readonly PayPalPayoutService _payPalPayoutService;

        private static Client _client { get; set; }
        private static Order _order { get; set; }

        public OrdersController (ApplicationDbContext context, UserService common, KoszykService koszykService, PayPalPayoutService payPalPayoutService)
        {
            _context = context;
            _common = common;
            _koszykService = koszykService;
            _payPalPayoutService = payPalPayoutService;
        }

        public async Task<IActionResult> Index ()
        {
            /*var user = await _common.GetUser (User);
            var orders = _context.Orders
                .Include(i=> i.OrderItems)
                .Where (w=> w.UserId == user.Id).ToList ();

            return View(orders);*/
            return View (new List<Order>() { });

        }


        [HttpGet]
        public async Task<IActionResult> Podsumowanie ()
        {
            return View(new PodsumowanieViewModel()
            {
                User = await _common.GetUser(User),
                KoszykItems = _koszykService.GetAll()
            });
        }



        [HttpGet]
        public async Task<IActionResult> Create ()
        {/*
            try
            {
                var koszykItems = _koszykService.GetAll();
                var user = await _common.GetUser (User);
                Order order = new Order ()
                {
                    OrderId = Guid.NewGuid ().ToString (),
                    UserId = user.Id,
                    Suma = koszykItems.Select (s=>s.Suma).Sum(),
                    StatusZamowienia = StatusZamowienia.WtrakcieRealizacji,
                    DataZamowienia = DateTime.Now
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();


                foreach (var koszykItem in koszykItems)
                {
                    OrderItem orderItem = new OrderItem ()
                    {
                        OrderItemId = Guid.NewGuid ().ToString (),
                        Ilosc = koszykItem.Ilosc,
                        Suma = koszykItem.Suma,
                        OrderId = order.OrderId,
                        ProductId = koszykItem.ProductId
                    };
                    _context.OrderItems.Add(orderItem);
                    await _context.SaveChangesAsync();
                }

                _koszykService.Clear();
            }
            catch { }*/

            return View();
        }




        [HttpGet]
        public async Task<IActionResult> OrderDetails (string orderId)
        {
            var orderItems = await _context.OrderItems
                .Include (i=> i.Product)
                .Where (w=> w.OrderId == orderId).ToListAsync ();

            return View(orderItems);
        }




        [HttpGet]
        public async Task<IActionResult> OrderFormularz ()
        { 
            if (_client != null)
            {
                ClientViewModel cvm = new ClientViewModel ()
                {
                    Imie = _client.Imie,
                    Nazwisko = _client.Nazwisko,
                    Ulica = _client.Ulica,
                    Miejscowosc = _client.Miejscowosc,
                    KodPocztowy = _client.KodPocztowy,
                    Kraj = _client.Kraj,
                    Telefon = _client.Telefon,
                    RodzajOsoby = _client.RodzajOsoby,
                    Newsletter = _client.Newsletter,
                    Email = _client.Email,
                };
                return View(cvm);
            }
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderFormularz (ClientViewModel model)
        {
            string orderId = "";
            if (ModelState.IsValid)
            { 
                    _client = new Client()
                    {
                        ClientId = Guid.NewGuid().ToString(),
                        Imie = model.Imie,
                        Nazwisko = model.Nazwisko,
                        Ulica = model.Ulica,
                        Miejscowosc = model.Miejscowosc,
                        KodPocztowy = model.KodPocztowy,
                        Kraj = model.Kraj,
                        Telefon = model.Telefon,
                        RodzajOsoby = model.RodzajOsoby,
                        Newsletter = model.Newsletter,
                        Email = model.Email,
                        DataDodania = DateTime.Now,
                    };

                return RedirectToAction("OrderPayments", "Orders", new { orderId = orderId });
            } 

            return View (model);
        }



        [HttpGet]
        public async Task<IActionResult> OrderPayments (PaymentsViewModel model)
        { 
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderPayments (string s, PaymentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                _order = new Order()
                {
                    OrderId = Guid.NewGuid().ToString(),
                    SposobPlatnosci = model.SposobPlatnosci,
                    SposobWysylki = model.SposobWysylki,
                    ClientId = _client.ClientId,
                    DataZamowienia = DateTime.Now,
                };
                 
                return RedirectToAction("OrderConfirmation", "Orders", new { orderId = model.OrderId });
            }
            return View (model);
        }




        [HttpGet]
        public async Task<IActionResult> OrderConfirmation ()
        {  
            return View (new OrderConfirmationViewModel()
            {
                Client = _client,
                Order = _order,
                KoszykItems = _koszykService.GetAll ()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Confirm ()
        {
            // Tworzy zamówienie
            try
            {
                if (_client != null && _order != null)
                {
                    _context.Clients.Add(_client);
                    await _context.SaveChangesAsync();

                    _order.Ilosc = _koszykService.GetAll().Sum(s => s.Ilosc);
                    _order.Suma = _koszykService.GetAll().Sum(s => s.Suma);
                    _order.StatusZamowienia = StatusZamowienia.Niezrealizowane;
                    _context.Orders.Add(_order);
                    await _context.SaveChangesAsync();

                    foreach (var koszykItem in _koszykService.GetAll())
                    {
                        OrderItem orderItem = new OrderItem ()
                        {
                            OrderItemId = Guid.NewGuid ().ToString (),
                            Ilosc = koszykItem.Ilosc,
                            Suma = koszykItem.Suma,
                            OrderId = _order.OrderId,
                            ProductId = koszykItem.ProductId
                        };
                        _context.OrderItems.Add(orderItem);
                        await _context.SaveChangesAsync();
                    }



                    // Uruchomienie okna płatności
                    string currency = "USD";
                    var orderId = await _payPalPayoutService.CreateOrder(Convert.ToDecimal(_order.Suma), currency);
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        _koszykService.Clear();
                        _client = null;
                        _order = null;

                        return Redirect($"https://www.sandbox.paypal.com/checkoutnow?token={orderId}");
                    }
                    else
                        ViewData["ErrorMessage"] = "Błąd podczas tworzenia zamówienia.";
                     

                }
            }
            catch { }

            return View ();
        }


        private async Task CreateOrder ()
        {
        }
         



    }
}
