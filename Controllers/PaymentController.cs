using Microsoft.AspNetCore.Mvc; 
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using WebApplication58.Services;

namespace WebApplication58.Controllers
{ 
    public class PaymentController : Controller
    {
        private readonly PayPalPayoutService _payPalPayoutService;

        public PaymentController (PayPalPayoutService payPalPayoutService)
        {
            _payPalPayoutService = payPalPayoutService;
        }

        public IActionResult Index ()
        {
            return View();
        }

        public async Task<IActionResult> CreateOrder ()
        {
            try
            {
                decimal amount = 1;
                string currency = "USD";

                var orderId = await _payPalPayoutService.CreateOrder(amount, currency);

                if (!string.IsNullOrEmpty(orderId))
                {
                    return Redirect($"https://www.sandbox.paypal.com/checkoutnow?token={orderId}");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Błąd podczas tworzenia zamówienia.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Błąd: {ex.Message}";
                return View();
            }
        }




        /*public async Task<IActionResult> CreateOrder ()
        {
            decimal amount = 1;
            string currency = "USD";

            try
            {
                var request = new OrdersCreateRequest()
                .Prefer("return=representation")
                .RequestBody(new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            AmountWithBreakdown = new AmountWithBreakdown
                            {
                                CurrencyCode = currency,
                                Value = amount.ToString("0.##")
                            }
                        }
                    },
                    ApplicationContext = new ApplicationContext
                    {
                        ReturnUrl = "https://example.com/success",
                        CancelUrl = "https://example.com/cancel"
                    }
                });

                var client = new PayPalHttpClient(new SandboxEnvironment(_clientId, _clientSecret));

                var response = await client.Execute(request);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var order = response.Result<Order>();
                    var orderId = order.Id;

                    // Pobierz link do zatwierdzenia płatności
                    var approvalLink = order.Links.Find(link => link.Rel.ToLower() == "approve")?.Href;

                    // Teraz możesz przekierować użytkownika do linku zatwierdzającego płatność
                    return Redirect(approvalLink);
                }
                else
                {
                    // Obsłuż błąd 
                    TempData["ErrorMessage"] = $"Utworzenie płatności nie powiodło się. Status Cod";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Obsłuż inne wyjątki
                TempData["ErrorMessage"] = $"Błąd: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
*/

        /*
                public async Task<IActionResult> CreateOrder (decimal amount, string currency = "USD")
                {
                    try
                    {
                        var request = new OrdersCreateRequest()
                        .Prefer("return=representation")
                        .RequestBody(new OrderRequest()
                        {
                            CheckoutPaymentIntent = "CAPTURE",
                            PurchaseUnits = new List<PurchaseUnitRequest>
                            {
                                new PurchaseUnitRequest
                                {
                                    AmountWithBreakdown = new AmountWithBreakdown
                                    {
                                        CurrencyCode = currency,
                                        Value = amount.ToString("0.##")
                                    }
                                }
                            },
                            ApplicationContext = new ApplicationContext
                            {
                                ReturnUrl = "https://example.com/success",
                                CancelUrl = "https://example.com/cancel"
                            }
                        });

                        var client = new PayPalHttpClient(new SandboxEnvironment(_clientId, _clientSecret));

                        var response = await client.Execute(request);

                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            var order = response.Result<Order>();
                            var orderId = order.Id;

                            // Pobierz link do zatwierdzenia płatności
                            var approvalLink = order.Links.Find(link => link.Rel.ToLower() == "approve")?.Href;

                            // Teraz możesz przekierować użytkownika do linku zatwierdzającego płatność
                            return Redirect(approvalLink);
                        }
                        else
                        {
                            // Obsłuż błąd 
                            TempData["ErrorMessage"] = $"Utworzenie płatności nie powiodło się. Status Cod";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Obsłuż inne wyjątki
                        TempData["ErrorMessage"] = $"Błąd: {ex.Message}";
                        return RedirectToAction("Index");
                    }
                }
        */




    }


}
