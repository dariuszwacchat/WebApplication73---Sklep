using Microsoft.AspNetCore.Authorization;
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
using WebApplication58.Services;
using WebApplication58.ViewModels;

namespace WebApplication58.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserService _userService;

        public UsersController (ApplicationDbContext context, UserService common, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userService = common;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index (UsersViewModel model)
        {
            var users = await _context.Users
                .Include(i => i.Client)
                    .ThenInclude (t=> t.Orders)
                .ToListAsync();

            return View(new UsersViewModel()
            {
                Paginator = Paginator<ApplicationUser>.CreateAsync(users, model.PageIndex, model.PageSize),
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
        public async Task<IActionResult> Index (string s, UsersViewModel model)
        {
            var users = await _context.Users
                .Include(i => i.Client)
                    .ThenInclude (t=> t.Orders)
                .ToListAsync();


            // Wyszukiwanie
            if (!string.IsNullOrEmpty(model.q))
                users = users.Where(w => w.Client.Nazwisko.Contains(model.q, StringComparison.OrdinalIgnoreCase)).ToList();


            // Sortowanie 
            switch (model.SortowanieOption)
            {
                case "Użytkownicy":
                    users = await PobierzUzytkownikowNaPodstawieRoli ("User");
                    break;

                case "Administratorzy":
                    users = await PobierzUzytkownikowNaPodstawieRoli("Administrator");
                    break;

                case "Zarząd":
                    users = await PobierzUzytkownikowNaPodstawieRoli("Zarząd");
                    break;

                case "Marketing":
                    users = await PobierzUzytkownikowNaPodstawieRoli("Marketing");
                    break;

                case "Wszystko":
                    break;
            }


            return View(new UsersViewModel()
            {
                Paginator = Paginator<ApplicationUser>.CreateAsync(users, model.PageIndex, model.PageSize),
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                Start = model.Start,
                End = model.End,
                q = model.q,
                SortowanieOption = model.SortowanieOption
            });
        }




        [HttpGet]
        public IActionResult Create ()
            => View(new CreateUserViewModel()
            {
                RegisterResult = ""
            });


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (CreateUserViewModel model)
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
                                Email = user.Email,
                                RodzajOsoby = model.RodzajOsoby,
                                Newsletter = model.Newsletter,
                                HasAccount = true,
                                DataDodania = DateTime.Now,
                            };
                            _context.Clients.Add(client);
                            await _context.SaveChangesAsync();


                            user.ClientId = client.ClientId;
                            await _userManager.UpdateAsync(user);


                            // dodanie nowozarejestrowanego użytkownika do ról 
                            foreach (var role in model.SelectedRoles)
                            {
                                await _userManager.AddToRoleAsync(user, role);
                                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                            }

                            // zalogowanie
                            //await _signInManager.SignInAsync(user, false);

                            model.RegisterResult = "Zarejestrowano, sprawdź pocztę aby dokończyć rejestrację";

                            return RedirectToAction ("Index", "Users");
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
        public async Task<IActionResult> Details (string userId)
        {
            var user = await _userService.GetUser (userId);

            if (user == null)
                return NotFound();

            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit (string userId)
        {
            var user = await _context.Users.
                Include (i => i.Client)
                .FirstOrDefaultAsync (f=> f.Id == userId);

            if (user == null)
                if (user.Client == null)
                    return NotFound(); 

            return View(new EditUserViewModel()
            {
                User = user,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    ApplicationUser user = await _context.Users.FirstOrDefaultAsync (f=> f.Id == model.User.Id);
                    if (user != null)
                    {
                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    Client client = await _context.Clients.FirstOrDefaultAsync(f=>f.ClientId == model.User.ClientId);
                    if (client != null)
                    {
                        client.Imie = model.User.Client.Imie;
                        client.Nazwisko = model.User.Client.Nazwisko;
                        client.Ulica = model.User.Client.Ulica;
                        client.Miejscowosc = model.User.Client.Miejscowosc;
                        client.KodPocztowy = model.User.Client.KodPocztowy;
                        client.Kraj = model.User.Client.Kraj;
                        client.Telefon = model.User.Client.Telefon;
                        client.Email = model.User.Email;
                        client.RodzajOsoby = model.User.Client.RodzajOsoby;
                        client.Newsletter = model.User.Client.Newsletter;

                        _context.Entry(client).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    // Usunięcie ze wszystkich rół
                    foreach (var role in await _context.Roles.ToListAsync ()) 
                        await _userManager.RemoveFromRoleAsync (user, role.Name); 


                    // Przypisanie nowych ról
                    foreach (var role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                    }
                     

                    return RedirectToAction("Index", "Users");
                }
                catch { }
            }

            return View(model);
        }
         


        [HttpGet]
        public async Task<IActionResult> Delete (string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync (f=> f.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync (f=> f.Id == id);

            if (user == null)
                return NotFound();

            var client = await _context.Clients.FirstOrDefaultAsync (f=> f.ClientId == user.ClientId);

            if (client == null)
                return NotFound();


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


            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            _context.Users.Remove (user);
            await _context.SaveChangesAsync ();

            return RedirectToAction ("Index", "Users");
        }



        private async Task<List<ApplicationUser>> PobierzUzytkownikowNaPodstawieRoli (string roleName)
        {
            List <ApplicationUser> usersInRole = new List<ApplicationUser> ();
            foreach (var user in await _context.Users.ToListAsync())
            {
                // Wszystkie role użytkownika
                var userRoles = await _userManager.GetRolesAsync(user);
                bool isInRole = userRoles.Contains (roleName);
                if (isInRole)
                    usersInRole.Add(user);
            }
            return usersInRole;
        }




    }
}
