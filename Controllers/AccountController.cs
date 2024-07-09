using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;
using WebApplication58.Models.Enum;
using WebApplication58.Services;
using WebApplication58.ViewModels;

namespace WebApplication58.Controllers
{ 
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager <ApplicationUser> _userManager;
        private readonly SignInManager <ApplicationUser> _signInManager;

        public AccountController (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index ()
        {
            Info.NavigationInfo = new NavigationInfo() { Navigation = Navigation.TwojeKonto };
            return View ();
        }
         
          
        [HttpGet]
        public async Task <IActionResult> DaneOsobowe ()
        {
            var user = await _userManager.GetUserAsync (User);
            return View (user);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login ()
            => View(new LoginViewModel() { LoginResult = "" });

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync (model.Email);
                if (user != null)
                {
                    var result =  await _signInManager.PasswordSignInAsync (user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        user.IloscZalogowan = user.IloscZalogowan + 1;
                        user.DataOstatniegoZalogowania = DateTime.Now.ToString ();
                        await _userManager.UpdateAsync (user);
                          

                        if (await _userManager.IsInRoleAsync(user, "Administrator"))
                            return Redirect("https://localhost:44390/Admin/Home");

                        if (await _userManager.IsInRoleAsync(user, "User"))
                            return RedirectToAction("Index", "Account");

                        
                        HttpContext.Session.Clear();
                        HttpContext.Session.SetString ("UserId", user.Id);
                    }
                    else
                    {
                        model.LoginResult = "Błędny login lub hasło";
                        return View(model);
                    }
                }
                else
                {
                    model.LoginResult = "User is null";
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout ()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register ()
            => View (new RegisterViewModel () { RegisterResult = "" });


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register (RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // sprawdza czy użytkownik o podanym mailu istnieje
                if ((await _userManager.FindByEmailAsync(model.Email)) == null)
                {
                    ApplicationUser user = new ApplicationUser ()
                    {
                        Id = Guid.NewGuid ().ToString (),

                        Email = model.Email,
                        UserName = model.Email,

                        ConcurrencyStamp = Guid.NewGuid ().ToString (),
                        SecurityStamp = Guid.NewGuid ().ToString (),
                        NormalizedEmail = model.Email.ToUpper(),
                        NormalizedUserName = model.Email.ToUpper(),
                        EmailConfirmed = false,
                        DataDodania = DateTime.Now,
                    };

                    var result = await _userManager.CreateAsync (user, model.Password);
                    if (result.Succeeded)
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
                            RodzajOsoby = model.RodzajOsoby,
                            Newsletter = model.Newsletter,
                            DataDodania = DateTime.Now,
                        };
                        _context.Clients.Add(client);
                        await _context.SaveChangesAsync();


                        user.ClientId = client.ClientId;
                        await _userManager.UpdateAsync(user);


                        // dodanie nowozarejestrowanego użytkownika do roli 
                        await _userManager.AddToRoleAsync(user, "User");
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

                        // zalogowanie
                        await _signInManager.SignInAsync(user, false);


                        model.RegisterResult = "Zarejestrowano, sprawdź pocztę aby dokończyć rejestrację";
                    }
                    else
                    {
                        model.RegisterResult = "Nie zarejestrowano.";
                    }
                }
                else
                {
                    model.RegisterResult = "Wskazany email już istnieje.";
                }

                // return RedirectToAction("Index", "Users");
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult ChangePassword ()
        {
            return View (new ChangePasswordViewModel() { ChangePasswordResult = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> ChangePassword (ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            { 
                ApplicationUser user = await _userManager.FindByNameAsync (User.Identity.Name);
                if (user == null)
                {
                    model.ChangePasswordResult = "Wskazany użytkownik nie istnieje";
                }
                else
                {
                    if (model.OldPassword != model.NewPassword)
                    {
                        IdentityResult result = await _userManager.ChangePasswordAsync (user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            model.ChangePasswordResult = "Hasło zmienione poprawnie";
                            //await _signInManager.SignOutAsync ();
                            //return RedirectToAction ("Login", "Home");
                        }
                    } 
                    else
                    {
                        model.ChangePasswordResult = "Hasła różnią się od siebie";
                    }
                    
                }
            }
            return View(model);
        }


        public async Task <IActionResult> DeleteAccount ()
        {
            ApplicationUser user = await _userManager.FindByNameAsync (User.Identity.Name);
            if (user != null)
            {
                IdentityResult result =  await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync ();
                    return RedirectToAction ("Index", "Home");
                }
            }
            else
            {
                // model.Result = "Wskazany użytkownik nie istnieje";
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
