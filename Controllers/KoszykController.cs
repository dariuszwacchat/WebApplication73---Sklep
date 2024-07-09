using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;
using WebApplication58.Services;

namespace WebApplication58.Controllers
{
    [AllowAnonymous]
    public class KoszykController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly KoszykService _koszykService;

        public KoszykController (ApplicationDbContext context, UserService userService, KoszykService koszykService)
        {
            _context = context;
            _userService = userService;
            _koszykService = koszykService;
        }

        public IActionResult Index ()
        { 
            return View(_koszykService.GetAll());
        }


        // Akcja znajduje się w kontrolerze Products
        /*[HttpGet] 
        public IActionResult Create (string productId)
        {
            var product = _context.Products.FirstOrDefault (f=> f.ProductId == productId);
            if (product != null)
                _koszykService.Create (product);

            return RedirectToAction ("Index", "Products");
        }*/


        [HttpGet] 
        public IActionResult Delete (string koszykItemId)
        {  
            _koszykService.Delete (koszykItemId);

            return RedirectToAction("Index", "Koszyk");
        }


    }
}
