using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;
using WebApplication58.ViewModels;

namespace WebApplication58.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager <ApplicationUser> _userManager;
        private readonly SignInManager <ApplicationUser> _signInManager;

        public AccountController (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login ()
            => View(new LoginViewModel() { LoginResult = "" });

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
                        return RedirectToAction("Index", "Home");
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


        [HttpGet]
        public IActionResult Register ()
            => View(new RegisterViewModel() { RegisterResult = "" });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register (RegisterViewModel model)
        {
            // sprawdza czy hasła są takie same
            if (model.Password == model.ConfirmPassword)
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
                            //await _signInManager.SignInAsync(user, false);


                            model.RegisterResult = "Zarejestrowano, sprawdź pocztę aby dokończyć rejestrację";
                        }
                    }
                    else
                    {
                        model.RegisterResult = "Wskazany email już istnieje.";
                    }

                    // return RedirectToAction("Index", "Users");
                }
            }
            else
            {
                model.RegisterResult = "Hasła muszą być takie same";
                return View(model);
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult ChangePassword ()
        {
            return View(new ChangePasswordViewModel() { ChangePasswordResult = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword (ChangePasswordViewModel model)
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

        public async Task<IActionResult> DeleteAccountByUserId (string applicationUserId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(applicationUserId);
            if (user != null)
            {
                IdentityResult result =  await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Users");
                }
            }
            else
            {
                //model.Result = "Wskazany użytkownik nie istnieje";
            }
            return RedirectToAction("Index", "Users");
        }


        public async Task<IActionResult> DeleteAccountByUserEmail (string userEmail)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                IdentityResult result =  await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Users");
                }
            }
            else
            {
                //model.Result = "Wskazany użytkownik nie istnieje";
            }
            return RedirectToAction("Index", "Users");
        }



    }
}
