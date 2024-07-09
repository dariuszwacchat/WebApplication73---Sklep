using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication58.Models;
using WebApplication58.Models.Enum;

namespace WebApplication58.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private DataAutogenerator.NetCore.DataAutogenerator _dataAutogenerator = new DataAutogenerator.NetCore.DataAutogenerator ();
        private Random _rand = new Random ();

        public ApplicationDbContext () { }
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
        }

        #region

        public DbSet<Client> Clients { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Subsubcategory> Subsubcategories { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<PhotoProduct> PhotosProduct { get; set; }
        public DbSet<ColorProduct> ColorsProduct { get; set; }
        public DbSet<SizeProduct> SizesProduct { get; set; }

        public DbSet<Favourite> Favourites { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Marka> Marki { get; set; }

        #endregion



        protected override void OnModelCreating (ModelBuilder builder)
        {

            builder.Entity<ApplicationUser>()
               .HasMany(h => h.Orders).WithOne(w => w.OsobaRealizujaca).HasForeignKey(f => f.OsobaRealizujacaId).OnDelete(DeleteBehavior.ClientSetNull);



            builder.Entity<Client>()
               .HasMany(h => h.Users).WithOne(w => w.Client).HasForeignKey(f => f.ClientId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Client>()
               .HasMany(h => h.Favourites).WithOne(w => w.Client).HasForeignKey(f => f.ClientId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Client>()
               .HasMany(h => h.Orders).WithOne(w => w.Client).HasForeignKey(f => f.ClientId).OnDelete(DeleteBehavior.ClientSetNull);



            builder.Entity<Category>()
                .HasMany(h => h.Subcategories).WithOne(w => w.Category).HasForeignKey(f => f.CategoryId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Subcategory>()
                .HasMany(h => h.Subsubcategories).WithOne(w => w.Subcategory).HasForeignKey(f => f.SubcategoryId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Subsubcategory>()
                .HasMany(h => h.Products).WithOne(w => w.Subsubcategory).HasForeignKey(f => f.SubsubcategoryId).OnDelete(DeleteBehavior.ClientSetNull);


            builder.Entity<Product>()
                .HasMany(h => h.OrderItems).WithOne(w => w.Product).HasForeignKey(f => f.ProductId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Product>()
                .HasMany(h => h.PhotosProduct).WithOne(w => w.Product).HasForeignKey(f => f.ProductId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Product>()
                .HasMany(h => h.ColorsProduct).WithOne(w => w.Product).HasForeignKey(f => f.ProductId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Product>()
                .HasMany(h => h.SizesProduct).WithOne(w => w.Product).HasForeignKey(f => f.ProductId).OnDelete(DeleteBehavior.ClientSetNull);

             
            builder.Entity<Order>()
                .HasMany(h => h.OrderItems).WithOne(w => w.Order).HasForeignKey(f => f.OrderId).OnDelete(DeleteBehavior.ClientSetNull);



            builder.Entity<Marka>()
                .HasMany(h => h.Produkty).WithOne(w => w.Marka).HasForeignKey(f => f.MarkaId).OnDelete(DeleteBehavior.ClientSetNull);





            CreateInitialData(builder);
            base.OnModelCreating(builder);
        }


        private void CreateInitialData (ModelBuilder builder)
        {


            // do 5 ponieważ jest 5 użytkowników w systemie na dzień dobry
            List <string> clientsId = new List<string> ();
            for (var i = 0; i < 15; i++)
            {
                Client client = new Client ()
                {
                    ClientId = Guid.NewGuid ().ToString (),
                    Imie = _dataAutogenerator.Imie(),
                    Nazwisko = _dataAutogenerator.Nazwisko (),
                    Ulica = _dataAutogenerator.Ulica (),
                    Miejscowosc = _dataAutogenerator.Nazwisko (),
                    KodPocztowy = "12-222",
                    Kraj = "Polska",
                    Telefon = "235235235",
                    Email = _dataAutogenerator.Email(),
                    DataDodania = DateTime.Now
                };
                builder.Entity<Client>().HasData(client);
                clientsId.Add (client.ClientId);
            }



            // ROLES 
            List <ApplicationRole> roles = new List<ApplicationRole> ()
            {
                new ApplicationRole()
                {
                    Id = Guid.NewGuid ().ToString (),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "User"
                },
                new ApplicationRole()
                {
                    Id = Guid.NewGuid ().ToString (),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Name = "Administrator",
                    NormalizedName = "Administrator"
                }
            };
            builder.Entity<ApplicationRole>().HasData(roles);



            // USERS  
            string photo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRvts5aHBstDkR8PigS4RmZkbZy78zpZoSuOw&usqp=CAU";

            PasswordHasher <ApplicationUser> adminPassword = new PasswordHasher<ApplicationUser> ();
            var administratorUser = new ApplicationUser ()
            {
                Id = Guid.NewGuid ().ToString (),
                Email = "admin@admin.pl",
                UserName = "admin@admin.pl",
                NormalizedUserName = "admin@admin.pl".ToUpper(),
                NormalizedEmail = "admin@admin.pl".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                ClientId = clientsId[0],
                /*Imie = _dataAutogenerator.Imie(),
                Nazwisko = _dataAutogenerator.Nazwisko(),
                Ulica = _dataAutogenerator.Ulica() + " 12",
                Miejscowość = _dataAutogenerator.Nazwisko (),
                Kraj = "Polska",
                KodPocztowy = "12-125",
                Telefon = "324453345",
                RodzajOsoby = RodzajOsoby.OsobaFizyczna,
                Photo = photo,
                IloscZalogowan = 0,
                DataDodania = DateTime.Now.ToString(),*/
            };
            administratorUser.PasswordHash = adminPassword.HashPassword(administratorUser, "SDG%$@5423sdgagSDert");
            IdentityUserRole <string> adminRole = new IdentityUserRole<string> ()
            {
                UserId = administratorUser.Id,
                RoleId = roles[1].Id
            };


            PasswordHasher <ApplicationUser> aaaPassword = new PasswordHasher<ApplicationUser> ();
            var aaaUser = new ApplicationUser ()
            {
                Id = Guid.NewGuid ().ToString (),
                
                Email = "aaa@aaa.pl",
                UserName = "aaa@aaa.pl",
                NormalizedUserName = "aaa@aaa.pl".ToUpper(),
                NormalizedEmail = "aaa@aaa.pl".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                IloscZalogowan = 0,
                DataDodania = DateTime.Now,
                ClientId = clientsId[1],
                /*Imie = _dataAutogenerator.Imie(),
                Nazwisko = _dataAutogenerator.Nazwisko(),
                Ulica = _dataAutogenerator.Ulica(),
                Miejscowość = _dataAutogenerator.Nazwisko (),
                Kraj = "Polska",
                KodPocztowy = "12-125",
                Telefon = "214 124 534",
                Plec = Plec.Mezczyzna,
                RodzajOsoby = RodzajOsoby.OsobaFizyczna,
                Photo = photo,
                DataDodania = DateTime.Now.ToString(),*/
            };
            aaaUser.PasswordHash = aaaPassword.HashPassword(aaaUser, "SDG%$@5423sdgagSDert");
            IdentityUserRole <string> aaaRole = new IdentityUserRole<string> ()
            {
                UserId = aaaUser.Id,
                RoleId = roles[0].Id
            };



            PasswordHasher <ApplicationUser> bbbPassword = new PasswordHasher<ApplicationUser> ();
            var bbbUser = new ApplicationUser ()
            {
                Id = Guid.NewGuid ().ToString (),
                Email = "bbb@bbb.pl",
                UserName = "bbb@bbb.pl",
                NormalizedUserName = "bbb@bbb.pl".ToUpper(),
                NormalizedEmail = "bbb@bbb.pl".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                IloscZalogowan = 0,
                ClientId = clientsId[2],
                DataDodania = DateTime.Now,
                /*Imie = _dataAutogenerator.Imie(),
                Nazwisko = _dataAutogenerator.Nazwisko(),
                Ulica = _dataAutogenerator.Ulica(),
                Miejscowość = _dataAutogenerator.Nazwisko (),
                Kraj = "Polska",
                KodPocztowy = "12-125",
                Telefon = "214 124 534",
                Plec = Plec.Mezczyzna,
                RodzajOsoby = RodzajOsoby.OsobaFizyczna,
                Photo = photo,
                DataDodania = DateTime.Now.ToString(),*/
            };
            bbbUser.PasswordHash = bbbPassword.HashPassword(bbbUser, "SDG%$@5423sdgagSDert");
            IdentityUserRole <string> bbbRole = new IdentityUserRole<string> ()
            {
                UserId = bbbUser.Id,
                RoleId = roles[0].Id
            };


            PasswordHasher <ApplicationUser> cccPassword = new PasswordHasher<ApplicationUser> ();
            var cccUser = new ApplicationUser ()
            {
                Id = Guid.NewGuid ().ToString (),
                Email = "ccc@ccc.pl",
                UserName = "ccc@ccc.pl",
                NormalizedUserName = "ccc@ccc.pl".ToUpper(),
                NormalizedEmail = "ccc@ccc.pl".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                IloscZalogowan = 0,
                ClientId = clientsId[3],
                DataDodania = DateTime.Now,
                /*Imie = _dataAutogenerator.Imie(),
                Nazwisko = _dataAutogenerator.Nazwisko(),
                Ulica = _dataAutogenerator.Ulica(),
                Miejscowość = _dataAutogenerator.Nazwisko (),
                Kraj = "Polska",
                KodPocztowy = "12-125",
                Telefon = "214 124 534",
                Plec = Plec.Mezczyzna,
                RodzajOsoby = RodzajOsoby.OsobaFizyczna,
                Photo = photo,
                DataDodania = DateTime.Now.ToString(),*/
            };
            cccUser.PasswordHash = cccPassword.HashPassword(cccUser, "SDG%$@5423sdgagSDert");
            IdentityUserRole <string> cccRole = new IdentityUserRole<string> ()
            {
                UserId = cccUser.Id,
                RoleId = roles[0].Id
            };


            PasswordHasher <ApplicationUser> dddPassword = new PasswordHasher<ApplicationUser> ();
            var dddUser = new ApplicationUser ()
            {
                Id = Guid.NewGuid ().ToString (),
                Email = "ddd@ddd.pl",
                UserName = "ddd@ddd.pl",
                NormalizedUserName = "ddd@ddd.pl".ToUpper(),
                NormalizedEmail = "ddd@ddd.pl".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                IloscZalogowan = 0,
                ClientId = clientsId[4],
                DataDodania = DateTime.Now,
                /*Imie = _dataAutogenerator.Imie(),
                Nazwisko = _dataAutogenerator.Nazwisko(),
                Ulica = _dataAutogenerator.Ulica(),
                Miejscowość = _dataAutogenerator.Nazwisko (),
                Kraj = "Polska",
                KodPocztowy = "12-125",
                Telefon = "214 124 534",
                Plec = Plec.Mezczyzna,
                RodzajOsoby = RodzajOsoby.OsobaFizyczna,
                Photo = photo,
                DataDodania = DateTime.Now.ToString(),*/
            };
            dddUser.PasswordHash = dddPassword.HashPassword(dddUser, "SDG%$@5423sdgagSDert");
            IdentityUserRole <string> dddRole = new IdentityUserRole<string> ()
            {
                UserId = dddUser.Id,
                RoleId = roles[0].Id
            };


            builder.Entity<ApplicationUser>().HasData(administratorUser, aaaUser, bbbUser, cccUser, dddUser);
            builder.Entity<IdentityUserRole<string>>().HasData(adminRole, aaaRole, bbbRole, cccRole, dddRole);


            List <ApplicationUser> users = new List<ApplicationUser> () {aaaUser, bbbUser, cccUser, dddUser };





            // POSTS

            List <string> photos = new List<string> ()
            {
                "https://www.protenis.com.pl/cache/files/240437348/pure-aero-98---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/240437348/rafa-290-2023---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/240437348/pure-aero-2023---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/928233567/1042a224-400-asics---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/928233567/1042a224-101-asics-2023---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/928233567/1042a165-404as-buty-tenisowe---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/928233567/1042a254-400-asics-buty-tenisowe-damskie---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/928233567/1042a217-400-3-9-2023---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/928233567/1042a134-104-3-2023-protenis-2---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/928233567/1042a198-101-asics-1.2023-1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1421207455/pilki-tenisowe-dunlop-atp-2019-oficial-ball-1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1421207455/wersje/pilki-tenisowe-dunlop-fort-clay-2019_16---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1421207455/dunlop-autralian-open-pilki-tenisowe-puszka_1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1175563111/dunlop-stage-3-red-12---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/717107113/tsgx-3-gosen-2022---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/717107113/goseng-tour3_2---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/717107113/83naciag-tenisowy-gosen-micros-super-220m-16g---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/780022970/283672-blge-head-2022-junior-1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1489540560/wr8017802-lime-wilson-junior---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/334667241/prince_durapro_2---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1174085877/karton-puszek-tenisowych-tecnifibre-club-nowe-logo---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1167813859/granatowe-owijki-toalson-ultra-grip-navy-3-szt---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1167813859/owijki-tenisowe-zewnetrzne-toalson-zielone_1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/693346931/1041a458-001.asics-2023---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/693346931/1041a330-101-asics-but-2023-1_1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/693346931/2023-asics-buty---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/693346931/1041a358-102---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/693346931/1041a224-401-asics---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/693346931/1041a330-400-asics-2023-asics-tenis---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/996149710/asics-gel-resolution-8-clay-1041a346-960-wyprzedaz---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/693346931/1041a299-001-1-asics-2022---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/318610289/1041a343-960-asics-1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/237343331/okulary_squash_tourna_2---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1014600267/head-evo-speed-23-new_1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1014600267/710100180-gravity-elite---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1804069569/koszyk-na-pilki-tenisowe-gamma-75_1---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1891842834/head-new-ball-trolley-additional-bag-2022---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1804069569/wozek-na-pilki-tenisowe---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/267313233/zestaw-trenerski-head-nowy---w-225-h-225-wo-225-ho-225.jpg",
                "https://www.protenis.com.pl/cache/files/1891842834/p176-ball-cart-czarny---w-225-h-225-wo-225-ho-225.jpg"
            };


            List <string> marki = new List<string> ()
            {
                "Babolat",
                "Head",
                "Lacoste",
                "Pacific",
                "Prince",
                "Pro Kennex",
                "Pro's Pro", 
            };
            List <string> markiId = new List<string> ();
            List <string> kolory = new List<string> () { "Biały", "Niebieski", "Zielony", "Czarny", "Szary" };
            List <string> rozmiary = new List<string> () { "33", "33.5", "34", "34.5", "35", "35.5" };

            // CATEGORY


            List <Category> categories = new List<Category> ()
            {
                new Category ()
                {
                    Name = "RakietyTenisowe",
                    FullName = "Rakiety tenisowe",
                    Kolejnosc = 1,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "Marka",
                            FullName = "Marka",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Lacoste",
                                    FullName = "Lacoste",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Pacific",
                                    FullName = "Pacific",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "ProKennex",
                                    FullName = "Pro Kennex",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "ProsPro",
                                    FullName = "Pro's Pro",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tencifibre",
                                    FullName = "Tencifibre",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Volkl",
                                    FullName = "Volkl",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex",
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "RakietyTenisoweWgTypu",
                            FullName = "Rakiety tenisowe wg. typu",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "DlaPoczatkujacych",
                                    FullName = "Dla poczatkujacych"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Rekreacyjne",
                                    FullName = "Rekreacyjne"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Klubowe",
                                    FullName = "Klubowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Sportowe",
                                    FullName = "Sportowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wypoczynkowe",
                                    FullName = "Wypoczynkowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Uzywane",
                                    FullName = "Używane"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "RakiedyJuniorskie",
                            FullName = "Rakiety Juniorski",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolad",
                                    FullName = "Babolad"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "ProsPro",
                                    FullName = "Pro's Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tecnifibre",
                                    FullName = "Tecnifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Volkl",
                                    FullName = "Volkl"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "RakietyTenisoweDlaDzieciWgCali",
                            FullName = "Rakiety tenisowe dla dzieci wg. cali",
                            Kolejnosc = 4,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "RakietyDlaDziecka17Cali",
                                    FullName = "Rakiety dla dziecka 17 cali"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "RakietyDlaDziecka19Cali",
                                    FullName = "Rakiety dla dziecka 19 cali"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "RakietyDlaDziecka21Cali",
                                    FullName = "Rakiety dla dziecka 21 cali"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "RakietyDlaDziecka23Cali",
                                    FullName = "Rakiety dla dziecka 23 cali"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "RakietyDlaDziecka24Cali",
                                    FullName = "Rakiety dla dziecka 24 cali"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "RakietyDlaDziecka25Cali",
                                    FullName = "Rakiety dla dziecka 25 cali"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "RakietyDlaDziecka26Cali",
                                    FullName = "Rakiety dla dziecka 26 cali"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "SerwisProtenis",
                            FullName = "Serwis Protenis",
                            Kolejnosc = 5,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "AkcesoriaDoTuningu",
                                    FullName = "Akcesoria do tuningu"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "TuningRakietTenisowych",
                                    FullName = "Tuning rakiet tenisowych"
                                },
                            }
                        }
                    }
                },
                /*new Category ()
                {
                    Name = "NaciagiTenisowe",
                    FullName = "Naciągi tenisowe",
                    Kolejnosc = 2,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "Marka",
                            FullName = "Marka",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Gamma",
                                    FullName = "Gamma",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Gosen",
                                    FullName = "Gosen",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Kirschbaum",
                                    FullName = "Kirschbaum",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Luxilon",
                                    FullName = "Luxilon",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "OneStrings",
                                    FullName = "One Strings",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Polystar",
                                    FullName = "Polystar",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SignumPro",
                                    FullName = "Signum Pro",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spinair",
                                    FullName = "Spinair",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Trening",
                                    FullName = "Trening",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tecnifibre",
                                    FullName = "Tecnifibre",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Topspin",
                                    FullName = "Topspin",
                                },
                                new Subsubcategory ()
                                {
                                    Name = "WeissCannon",
                                    FullName = "Weiss Cannon",
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "TOP8NajczesciejWybierane",
                            FullName = "TOP 8 - najczęściej wybierane",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "LuxilonAluPowerRough",
                                    FullName = "Luxilon Alu Power Rough"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "BabolatRPMBlast",
                                    FullName = "Babolat RPM Blast"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "BabolatRPMBlast",
                                    FullName = "Babolat RPM Blast"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SignumProTornado",
                                    FullName = "Signum Pro Tornado"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "YonexPolyTourProYellow",
                                    FullName = "Yonex Poly Tour Pro Yellow"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "LuxilonAluPowerSilver",
                                    FullName = "Luxilon Alu Power Silver"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "VolklCyclone",
                                    FullName = "Volkl Cyclone"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SignumProFirestorm",
                                    FullName = "Signum Pro Firestorm"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Dlugosc",
                            FullName = "Długość",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Sety",
                                    FullName = "Sety"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SetyHybrydy",
                                    FullName = "Sety / hybrydy"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Szpule",
                                    FullName = "Szpule"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SzpuleHybrydy",
                                    FullName = "Szpule / hybrydy"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Wlasciwosci",
                            FullName = "Właściwości",
                            Kolejnosc = 4,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Komfort",
                                    FullName = "Komfort"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wytrzymalosc",
                                    FullName = "Wytrzymałość"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Rotacja",
                                    FullName = "Rotacja"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Dynamika",
                                    FullName = "Dynamika"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Kontrola",
                                    FullName = "Kontrola"
                                },
                            }
                        }
                    }
                },
                new Category ()
                {
                    Name = "PilkiTenisowe",
                    FullName = "Piłki tenisowe",
                    Kolejnosc = 3,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            SubcategoryId = Guid.NewGuid ().ToString (),
                            Name = "Marka",
                            FullName = "Marka",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Gamma",
                                    FullName = "Gamma"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Pacific",
                                    FullName = "Pacific"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "ProsPro",
                                    FullName = "Pro's Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Slazenger",
                                    FullName = "Slazenger"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tretorn",
                                    FullName = "Tretorn"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Volkl",
                                    FullName = "Volkl"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Dunlop",
                                    FullName = "Dunlop"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            SubcategoryId = Guid.NewGuid ().ToString (),
                            Name = "PiłkiTenisoweWkartonach",
                            FullName = "Piłki tenisowe w kartonach",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "BallsUnlimited",
                                    FullName = "Balls Unlimited"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Dunlop",
                                    FullName = "Dunlop"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Slazenger",
                                    FullName = "Slazenger"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tecnifibre",
                                    FullName = "Tecnifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tretorn",
                                    FullName = "Tretorn"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            SubcategoryId = Guid.NewGuid ().ToString (),
                            Name = "Rodzaj",
                            FullName = "Rodzaj",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "PilkiTurniejowe",
                                    FullName = "Piłki turniejowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "PiłkiTreningowe",
                                    FullName = "Piłki treningowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "PilkiBezcisnienioweTreningowe",
                                    FullName = "Piłki bezciśnieniowe treningowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "PilkiDlaDzieci",
                                    FullName = "Piłki dla dzieci"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "PilkiWwiadrachWorkach",
                                    FullName = "Piłki w wiadrach, workach, itp."
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Klasa",
                            FullName = "Klasa",
                            Kolejnosc = 4,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Standard",
                                    FullName = "Standard"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Premium",
                                    FullName = "Premium"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "PilkiNaAutografy",
                            FullName = "Piłki na autografy",
                            Kolejnosc = 5,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "ProsPro",
                                    FullName = "Pro's Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                            }
                        },
                    }
                },
                new Category ()
                {
                    Name = "TorbyTenisowe",
                    FullName = "Torby tenisowe",
                    Kolejnosc = 4,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "Marka",
                            FullName = "Marka",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Dunlop",
                                    FullName = "Dunlop"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Hydrogen",
                                    FullName = "Hydrogen"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SignumPro",
                                    FullName = "Signum Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tencifibre",
                                    FullName = "Tencifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Volkl",
                                    FullName = "Volkl"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Kategoria",
                            FullName = "Kategoria",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "6Rakiet",
                                    FullName = "6 Rakiet"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "10Rakiet",
                                    FullName = "10 Rakiet"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "NaLaptop",
                                    FullName = "Na laptop"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Juniorskie",
                                    FullName = "Juniorskie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Pokrowce",
                                    FullName = "Pokrowce"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "3Rakiety",
                                    FullName = "3 Rakiety"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "9Rakiet",
                                    FullName = "9 Rakiet"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "1215Rakiet",
                                    FullName = "12 - 15 Rakiet"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Treningowe",
                                    FullName = "Treningowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Podrozne",
                                    FullName = "Podróżne"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Plecaki",
                            FullName = "Plecaki",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Hydrogen",
                                    FullName = "Hydrogen"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "KSwiss",
                                    FullName = "K-Swiss"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Pacific",
                                    FullName = "Pacific"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tecnifibre",
                                    FullName = "Tecnifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Volkl",
                                    FullName = "Volkl"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "PokrowceNaButySaszetki",
                            FullName = "Pokrowce na bury, saszetki",
                            Kolejnosc = 4,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolt",
                                    FullName = "Babolt"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tecnifibre",
                                    FullName = "Tecnifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                }
                            }
                        }
                    }
                },
                new Category ()
                {
                    Name = "Owijki",
                    FullName = "Owijki",
                    Kolejnosc = 5,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "OwijakiZewnetrzne",
                            FullName = "Owijaki zewnętrzne",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "MSV",
                                    FullName = "MSV"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SignumPro",
                                    FullName = "Signum Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tencifibre",
                                    FullName = "Tencifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "OwijakiBazowe",
                            FullName = "Owijaki bazowe",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Gamma",
                                    FullName = "Gamma"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Lacoste",
                                    FullName = "Lacoste"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Pacific",
                                    FullName = "Pacific"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "ProsPro",
                                    FullName = "Pro's Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tecnifibre",
                                    FullName = "Tecnifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "MSV",
                                    FullName = "MSV"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SignumPro",
                                    FullName = "SignumPro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tourna",
                                    FullName = "Tourna"
                                },
                            }
                        }
                    }
                },
                new Category ()
                {
                    Name = "Buty",
                    FullName = "Buty",
                    Kolejnosc = 6,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "ButyTenisoweDamskie",
                            FullName = "Buty tenisowe damskie",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Lotto",
                                    FullName = "Lotto"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Asics",
                                    FullName = "Asics"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Fila",
                                    FullName = "Fila"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "KSwiss",
                                    FullName = "K-Swiss"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Mizuno",
                                    FullName = "Mizuno"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Yonex",
                                    FullName = "Yonex"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "ButyTenisoweMeskie",
                            FullName = "Buty tenisowe męskie",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Mizuno",
                                    FullName = "Mizuno"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Asics",
                                    FullName = "Asics"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Lotto",
                                    FullName = "Lotto"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "ButyTenisoweJuniorskie",
                            FullName = "Buty tenisowe juniorskie",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Asics",
                                    FullName = "Asics"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Lotto",
                                    FullName = "Lotto"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Adidas",
                                    FullName = "Adidas"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Mizuno",
                                    FullName = "Mizuno"
                                },
                            }
                        },
                    }
                },
                new Category ()
                {
                    Name = "Odziez",
                    FullName = "Odzież",
                    Kolejnosc = 7,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "OdziezDamska",
                            FullName = "Odzież damska",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Koszulki",
                                    FullName = "Koszulki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodniczki",
                                    FullName = "Spódniczki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Sukienki",
                                    FullName = "Sukienki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodenki",
                                    FullName = "Spodenki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "DresySpodnie",
                                    FullName = "Desy, spodnie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Bluzy",
                                    FullName = "Bluzy"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "OdziezDamska",
                            FullName = "Odzież męska",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Koszulki",
                                    FullName = "Koszulki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodenki",
                                    FullName = "Spodenki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Bluzy",
                                    FullName = "Bluzy"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodnie",
                                    FullName = "Spodnie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Dresy",
                                    FullName = "Dresy"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Akcesoria",
                            FullName = "Akcesoria",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "FrotkiNaNadgarstki",
                                    FullName = "Frotki na nadgarstki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "FrotkiNaNadgarstki",
                                    FullName = "Frotki na nadgarstki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "CzapkiTenisowe",
                                    FullName = "Czapki tenisowe"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Reczniki",
                                    FullName = "Ręczniki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Skarpety",
                                    FullName = "Skarpety"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "OdziezDziewczeca",
                            FullName = "Odzież dziewczęca",
                            Kolejnosc = 4,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Koszulki",
                                    FullName = "Koszulki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodniczki",
                                    FullName = "Spódniczki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodenki",
                                    FullName = "Spodenki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodnie",
                                    FullName = "Spodnie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Sukienki",
                                    FullName = "Sukienki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Bluzy",
                                    FullName = "Bluzy"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Asics",
                                    FullName = "Asics"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "OdziezChlopieca",
                            FullName = "Odzież chłopięca",
                            Kolejnosc = 5,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Koszulki",
                                    FullName = "Koszulki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodenki",
                                    FullName = "Spodenki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Bluzy",
                                    FullName = "Bluzy"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Spodnie",
                                    FullName = "Spodnie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Dresy",
                                    FullName = "Dresy"
                                },
                            }
                        },
                    }

                },
                new Category ()
                {
                    Name = "Akcesoria",
                    FullName = "Akcesoria",
                    Kolejnosc = 8,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "MaszynyDoNaciagania",
                            FullName = "Maszyny do naciągania",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> () { }
                        },
                        new Subcategory ()
                        {
                            Name = "Wibrastopy",
                            FullName = "Wibrastopy",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Babolat",
                                    FullName = "Babolat"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Gamma",
                                    FullName = "Gamma"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Luxilon",
                                    FullName = "Luxilon"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Pacific",
                                    FullName = "Pacific"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "PrisPro",
                                    FullName = "Pri's Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tencifibre",
                                    FullName = "Tencifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tourna",
                                    FullName = "Tourna"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tencifibre",
                                    FullName = "Tencifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wilson",
                                    FullName = "Wilson"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Volkl",
                                    FullName = "Volkl"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "PomocTrenerska",
                            FullName = "Pomoc trenerska",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "KoszeTrenerski",
                                    FullName = "Kosze trenerskie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tuby",
                                    FullName = "Tuby"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Obuwie",
                                    FullName = "Obuwie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "DoCwiczen",
                                    FullName = "Do ćwiczeń"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "DrabinkiMaty",
                                    FullName = "Drabinki, maty"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "DyskiPacholkiTyczki",
                                    FullName = "Dyski, pachłki, tyczki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "LinieIznaczniki",
                                    FullName = "Linie i znaczniki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "PilkiLekarskie",
                                    FullName = "Piłki lekarskie, gimnastyczne"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Plotki",
                                    FullName = "Płotki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Siatki",
                                    FullName = "Siatki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Skakanki",
                                    FullName = "Skakanki"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Pozostale",
                            FullName = "Pozostałe",
                            Kolejnosc = 4,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "BreloczkiGadzety",
                                    FullName = "Breloczki, gadżety"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Elastokrosy",
                                    FullName = "Elastokrosy"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "MiernikiSilyNaciagu",
                                    FullName = "Mierniki siły naciągu"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Narzedzia",
                                    FullName = "Narzęzie"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "PreparatyDoDloni",
                                    FullName = "Preparaty do dłoni"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Rekawiczki",
                                    FullName = "Rękawiczki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "TasmyIzakonczeniaDoOwijek",
                                    FullName = "Taśmy i zakończenia do owijek"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tuning",
                                    FullName = "Tuning"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Zegarki",
                                    FullName = "Zegarki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "LotkiDoBadmintona",
                                    FullName = "Lotki do badmintona"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SiatkiDoTenisaStolowego",
                                    FullName = "Siatki do tenisa stolowego"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Bidony",
                                    FullName = "Bidony"
                                },
                            }
                        },
                    }
                },
                new Category ()
                {
                    Name = "Padel",
                    FullName = "Padel",
                    Kolejnosc = 9,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "Rakiety",
                            FullName = "Rakiety",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> () { }
                        },
                        new Subcategory ()
                        {
                            Name = "Pilki",
                            FullName = "Piłki",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> () { }
                        },
                        new Subcategory ()
                        {
                            Name = "Torby",
                            FullName = "Torby",
                            Kolejnosc = 3,
                            Subsubcategories = new List<Subsubcategory> () { }
                        },
                    }
                },
                new Category ()
                {
                    Name = "Squash",
                    FullName = "Squash - %",
                    Kolejnosc = 10,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "Naciagi",
                            FullName = "Naciągi",
                            Kolejnosc = 1,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Prince",
                                    FullName = "Prince"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "SignumPro",
                                    FullName = "Signum Pro"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Head",
                                    FullName = "Head"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tecnifibre",
                                    FullName = "Tecnifibre"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Tourna",
                                    FullName = "Tourna"
                                },
                            }
                        },
                        new Subcategory ()
                        {
                            Name = "Akcesoria",
                            FullName = "Akcesoria",
                            Kolejnosc = 2,
                            Subsubcategories = new List<Subsubcategory> ()
                            {
                                new Subsubcategory ()
                                {
                                    Name = "Owijki",
                                    FullName = "Owijki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Wibrastopy",
                                    FullName = "Wibrastopy"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Piłki",
                                    FullName = "Piłki"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "Okulary",
                                    FullName = "Okulary"
                                },
                                new Subsubcategory ()
                                {
                                    Name = "OdziezPromocyjna",
                                    FullName = "Odzież promocyjna"
                                },
                            }
                        },
                    }
                },
                new Category ()
                {
                    Name = "Wyprzedaz",
                    FullName = "Wyprzedaż",
                    Kolejnosc = 11,
                    Subcategories = new List<Subcategory> ()
                    {
                        new Subcategory ()
                        {
                            Name = "",
                            FullName = "",
                            Subsubcategories = new List<Subsubcategory> () { }
                        }
                    }
                },*/
            };


            foreach (var kolor in kolory)
            {
                Color color = new Color ()
                {
                    ColorId = Guid.NewGuid ().ToString (),
                    Name = kolor
                };
                builder.Entity<Color>().HasData(color);
            }


            foreach (var rozmiar in rozmiary)
            {
                Size size = new Size ()
                {
                    SizeId = Guid.NewGuid ().ToString (),
                    Name = rozmiar
                };
                builder.Entity<Size>().HasData(size);
            }

            foreach (var marka in marki)
            {
                Marka m = new Marka ()
                {
                    MarkaId = Guid.NewGuid ().ToString (),
                    Name = marka
                };
                builder.Entity<Marka>().HasData(m);
                markiId.Add(m.MarkaId);
            }



            List <string> productsId = new List<string> ();
            foreach (Category category in categories)
            {
                Category c = new Category ()
                {
                    CategoryId = Guid.NewGuid().ToString(),
                    Name = category.Name,
                    FullName = category.FullName,
                    IloscOdwiedzin = 0,
                    Kolejnosc = category.Kolejnosc,
                };
                builder.Entity<Category>().HasData(c);



                foreach (Subcategory subcategory in category.Subcategories)
                {
                    Subcategory s = new Subcategory ()
                    {
                        SubcategoryId = Guid.NewGuid().ToString (),
                        Name = subcategory.Name,
                        FullName = subcategory.FullName,
                        Kolejnosc = subcategory.Kolejnosc,
                        IloscOdwiedzin = 0,
                        CategoryId = c.CategoryId
                    };
                    builder.Entity<Subcategory>().HasData(s);




                    if (subcategory.Subsubcategories != null)
                    {

                        foreach (Subsubcategory subsubcategory in subcategory.Subsubcategories)
                        {
                            Subsubcategory ss = new Subsubcategory ()
                            {
                                SubsubcategoryId = Guid.NewGuid().ToString (),
                                Name = subsubcategory.Name,
                                FullName = subsubcategory.FullName,
                                Kolejnosc = subsubcategory.Kolejnosc,
                                IloscOdwiedzin = 0,
                                SubcategoryId = s.SubcategoryId,
                                CategoryId = c.CategoryId
                            };
                            builder.Entity<Subsubcategory>().HasData(ss);


                            for (var l = 0; l < _rand.Next(1, 4); l++)
                            {
                                Product p = new Product ()
                                {
                                    ProductId = Guid.NewGuid ().ToString (),
                                    Name = _dataAutogenerator.Nazwisko (),
                                    Description = _dataAutogenerator.Description (10),
                                    Price = _dataAutogenerator.Price(40,250),
                                    IloscOdwiedzin = 0,
                                    Quantity = _dataAutogenerator.Number (),
                                    Znizka = _rand.Next (10,20),
                                    Kolor = kolory[_rand.Next(0, kolory.Count)],
                                    Rozmiar = rozmiary[_rand.Next(0,rozmiary.Count)],
                                    DataDodania = _dataAutogenerator.RandomDateTime ().ToString (),
                                    CategoryId = c.CategoryId,
                                    MarkaId = markiId[_rand.Next (0,markiId.Count)],
                                    SubcategoryId = s.SubcategoryId,
                                    SubsubcategoryId = ss.SubsubcategoryId
                                };
                                builder.Entity<Product>().HasData(p);

                                productsId.Add (p.ProductId);

                                // Dodanie kolorów do produktu
                                for (var i = 0; i < _rand.Next(1, 3); i++)
                                {
                                    ColorProduct colorProduct = new ColorProduct ()
                                    {
                                        ColorProductId = Guid.NewGuid ().ToString (),
                                        Name = kolory [_rand.Next(0,kolory.Count)],
                                        ProductId = p.ProductId
                                    };
                                    builder.Entity<ColorProduct>().HasData(colorProduct);
                                }

                                // Dodanie rozmiarów do produktu
                                for (var i = 0; i < _rand.Next(1, 3); i++)
                                {
                                    SizeProduct sizeProduct = new SizeProduct ()
                                    {
                                        SizeProductId = Guid.NewGuid ().ToString (),
                                        Name = rozmiary [_rand.Next(0,rozmiary.Count)],
                                        ProductId = p.ProductId
                                    };
                                    builder.Entity<SizeProduct>().HasData(sizeProduct);
                                }

                            }

                        }
                    }


                }

            }


/*
            // Dodanie zdjęć do produktu
            foreach (var productId in productsId)
            {
                for (var i = 0; i < _rand.Next(1, 4); i++)
                {
                    PhotoProduct photoProduct = new PhotoProduct ()
                    {
                        PhotoProductId = Guid.NewGuid ().ToString (),
                        PhotoData = GetImageBytesAsync(photos [_rand.Next(0,photos.Count)]),
                        ProductId = productId
                    };
                    builder.Entity<PhotoProduct>().HasData(photoProduct);
                }
            }
*/

        }



        /// <summary>
        /// Zamienia zdjęcie pobrane z siebie na byte[]
        /// </summary>
        private byte[] GetImageBytesAsync (string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                byte[] imageBytes;

                using (var response = httpClient.GetAsync(imageUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                    }
                    else
                    {
                        imageBytes = new byte[0];
                    }
                }

                return imageBytes;
            }
        }

    }
}