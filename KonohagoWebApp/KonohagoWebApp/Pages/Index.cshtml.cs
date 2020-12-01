using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KonohagoWebApp.Helpers;
using KonohagoWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using KonohagoWebApp.Repository.Interfaces;
namespace KonohagoWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet()
        {
            if (HttpContext.Request.Cookies.ContainsKey("email") && HttpContext.Request.Cookies.ContainsKey("password"))
            {
                var repository = HttpContext.RequestServices.GetService<IUserRepository>();
                var email = HttpContext.Request.Cookies["email"];
                var password = HttpContext.Request.Cookies["password"];
                var user = await repository.GetUserByEmailAndPasswordAsync(email, password);
                var a = HttpContext.Session.GetString("role");
                HttpContext.Session.Set<User>("Current_user", user);
            }
            if (HttpContext.Session.GetString("role") == "Guest")
            {
                ViewData["name"] = "Путник";
                ViewData["role"] = "Guest";
            }
            else if (HttpContext.Session.GetString("role") == "User")
            {
                var user = HttpContext.Session.Get<User>("Current_user");
                ViewData["name"] = user.Nickname;
                ViewData["role"] = "User";
            }
        }
        public IActionResult OnPostClearSession()
        {
            HttpContext.Session.Clear();
            if (HttpContext.Request.Cookies["email"] != null || HttpContext.Request.Cookies["password"]!= null)
            {
                HttpContext.Response.Cookies.Delete("email");
                HttpContext.Response.Cookies.Delete("password");
            }

            return Redirect("/Index");
        }
    }
}
