using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;

namespace WebApplication58.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context; 

        public UserService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List <ApplicationUser>> GetUsers ()
        {
            var users = await _context.Users 
                .ToListAsync ();
            return users;
        }

        public async Task <ApplicationUser> GetUser (ClaimsPrincipal userClaimsPrincipal)
        { 
            var user = await _context.Users 
                .Include(i=> i.Client)
                .FirstOrDefaultAsync (f=> f.UserName == userClaimsPrincipal.Identity.Name);
            return user;
        }

        public async Task<ApplicationUser> GetUser (string userId)
        { 
            var user = await _context.Users
                .FirstOrDefaultAsync (f=> f.Id == userId);
            return user;
        }
         



    }
}
