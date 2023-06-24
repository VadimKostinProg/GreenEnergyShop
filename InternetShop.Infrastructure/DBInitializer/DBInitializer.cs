using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
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

        private readonly IArticleService _articleService;

        private readonly IConfiguration _config;

        public DBInitializer(UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager, ApplicationDbContext db, IConfiguration configuration,
        IArticleService articleService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _config = configuration;
            _articleService = articleService;
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



            if(_articleService.GetArticleByTitle("Про нас").GetAwaiter().GetResult() == null)
            {
                var articleAddRequest = new ArticleAddRequest()
                {
                    Title = "Про нас",
                    Description = string.Empty
                };

                _articleService.AddArticle(articleAddRequest).GetAwaiter().GetResult();
            }

            if (_articleService.GetArticleByTitle("Доставка та оплата").GetAwaiter().GetResult() == null)
            {
                var articleAddRequest = new ArticleAddRequest()
                {
                    Title = "Доставка та оплата",
                    Description = string.Empty
                };

                _articleService.AddArticle(articleAddRequest).GetAwaiter().GetResult();
            }

            if (_articleService.GetArticleByTitle("Контакти").GetAwaiter().GetResult() == null)
            {
                var articleAddRequest = new ArticleAddRequest()
                {
                    Title = "Контакти",
                    Description = string.Empty
                };

                _articleService.AddArticle(articleAddRequest).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
