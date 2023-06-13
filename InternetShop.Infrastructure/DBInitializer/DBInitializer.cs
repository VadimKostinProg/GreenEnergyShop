using InternetShop.Infrastructure.DataBaseContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Infrastructure.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        private readonly IConfiguration _config;

        public DBInitializer(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, ApplicationDbContext db, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _config = configuration;
        }

        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex) { }

            if(!_roleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

                string email = _config.GetValue<string>("AdminData:Email");
                string password = _config.GetValue<string>("AdminData:Password");

                _userManager.CreateAsync(new IdentityUser()
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = "+3802354396"
                }, password).GetAwaiter().GetResult();

                IdentityUser user = _userManager.FindByEmailAsync("tolkostin2@gmail.com").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            }

            return;
        }
    }
}
