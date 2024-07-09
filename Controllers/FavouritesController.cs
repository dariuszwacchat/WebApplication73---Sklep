using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;

namespace WebApplication58.Controllers
{
    [Authorize]
    public class FavouritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager <ApplicationUser> _userManager;
        public FavouritesController (ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
          
/*
        public async Task<IActionResult> Create (string productId)
        {
            var user = await _context.Users
                .Include(i=> i.Favourites)
                .FirstOrDefaultAsync (f=> f.UserName == User.Identity.Name);
              
            if (!await CzyFavouriteDodany (user.Id, productId))
                await DodajFavourite (user.Id, productId); 

            return RedirectToAction("ProductDetails", "Home", new { productId = productId });
        }

        public async Task<IActionResult> Delete (string favouriteId)
        {
            if (string.IsNullOrEmpty (favouriteId))
                return NotFound ();  
            var favourite = await _context.Favourites.FirstOrDefaultAsync (f=> f.FavouriteId == favouriteId);
           
            if (favourite == null) 
                return NotFound ();

            _context.Favourites.Remove (favourite);
            await _context.SaveChangesAsync ();

            return RedirectToAction ("Ulubione", "Account");
        }
         

        private async Task DodajFavourite (string userId, string productId)
        {
            try
            {
                Favourite favourite = new Favourite ()
                {
                    FavouriteId = Guid.NewGuid ().ToString (),
                    UserId = userId,
                    ProductId = productId
                };
                _context.Favourites.Add(favourite);
                await _context.SaveChangesAsync ();
            }
            catch { }
        }
          
        private async Task<bool> CzyFavouriteDodany (string userId, string productId)
        {
            var favourite = await _context.Favourites.FirstOrDefaultAsync (f=> f.UserId == userId && f.ProductId == productId);
            if (favourite == null)
                return false;
            else
                return true;
        }
*/





    }
}
