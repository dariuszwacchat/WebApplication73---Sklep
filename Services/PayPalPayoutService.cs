using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayoutsSdk.Payouts;
using PayPalHttp;

namespace WebApplication58.Services
{

    public class PayPalPayoutService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public PayPalPayoutService (IConfiguration configuration)
        {
            _clientId = configuration["PayPal:ClientId"];
            _clientSecret = configuration["PayPal:ClientSecret"];
        }

        public async Task<string> CreateOrder (decimal amount, string currency = "USD")
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
                        ReturnUrl = "https://localhost:44390/Home/Success",
                        CancelUrl = "https://localhost:44390/Home/Cancel"
                    }
                });

                var client = new PayPalHttpClient(new SandboxEnvironment(_clientId, _clientSecret));

                var response = await client.Execute(request);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var order = response.Result<Order>();
                    return order.Id;
                }
                else
                {
                    // Obsłuż błąd
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Obsłuż błąd
                return null;
            }
        }


    }
}
