using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;

namespace WebApplication58.Services
{
    public class KoszykService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKeyPrefix = "UserCart_";

        public KoszykService (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<KoszykItem> GetAll ()
        {
            string userId = GetUserId();
            List<KoszykItem> deserializeProducts = new List<KoszykItem>();
            try
            {
                string sessionKey = GetSessionKey (userId);
                string shoppingCart = _httpContextAccessor.HttpContext.Session.GetString (sessionKey);
                if (!string.IsNullOrEmpty(shoppingCart))
                    deserializeProducts = JsonConvert.DeserializeObject<List<KoszykItem>>(shoppingCart);
            }
            catch { }

            return deserializeProducts ?? new List<KoszykItem>();
        }

        public void Create (Product product)
        {
            string userId = GetUserId();
            try
            {
                List<KoszykItem> koszykItems = GetAll();

                // Sprawdź, czy produkt już istnieje w koszyku
                var existingItem = koszykItems.FirstOrDefault(item => item.ProductId == product.ProductId);

                if (existingItem != null)
                {
                    // Jeśli produkt już istnieje, zaktualizuj ilość
                    existingItem.Ilosc++;
                    existingItem.Suma += existingItem.Suma;
                }
                else
                {
                    // Jeśli produkt nie istnieje, utwórz nowy element koszyka
                    KoszykItem koszykItem = new KoszykItem()
                    {
                        KoszykItemId = Guid.NewGuid().ToString(),
                        Ilosc = 1,
                        Suma = product.Price,
                        ProductId = product.ProductId,
                        Product = product
                    };

                    koszykItems.Add(koszykItem);
                }

                // Zapisz aktualny koszyk w sesji
                string sessionKey = GetSessionKey (userId);
                string serializeProduct = JsonConvert.SerializeObject(koszykItems);
                _httpContextAccessor.HttpContext.Session.SetString(sessionKey, serializeProduct);
                  
            }
            catch { }
        }

        public void Delete (string koszykItemId)
        {
            string userId = GetUserId();
            try
            {
                List<KoszykItem> koszykItems = GetAll();

                // Usuń element o określonym identyfikatorze z koszyka
                var koszykItem = koszykItems.FirstOrDefault(item => item.KoszykItemId == koszykItemId);
                if (koszykItem != null)
                {
                    koszykItems.Remove(koszykItem);

                    // Zapisz aktualny koszyk w sesji
                    string sessionKey = GetSessionKey (userId);
                    string serializeProduct = JsonConvert.SerializeObject(koszykItems);
                    _httpContextAccessor.HttpContext.Session.SetString(sessionKey, serializeProduct);
                }
            }
            catch { }
        }


        public void Clear ()
        {
            string userId = GetUserId();
            try
            {
                List<KoszykItem> koszykItems = GetAll();
                koszykItems.Clear();

                // Zapisz aktualny koszyk w sesji
                string sessionKey = GetSessionKey (userId);
                string serializeProduct = JsonConvert.SerializeObject(koszykItems);
                _httpContextAccessor.HttpContext.Session.SetString(sessionKey, serializeProduct);
            }
            catch { }
        }
         

        private string GetUserId ()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        private string GetSessionKey (string userId)
        {
            return string.IsNullOrEmpty(userId) ? "GuestCart" : SessionKeyPrefix + userId;
        }

    }

}