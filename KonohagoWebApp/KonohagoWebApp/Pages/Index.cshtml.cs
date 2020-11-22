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
namespace KonohagoWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("role") == "Guest")
            {
                ViewData["name"] = "Guest";
            }
            else if (HttpContext.Session.GetString("role") == "User")
            {
                var user = HttpContext.Session.Get<User>("Current_user");
                ViewData["name"] = user.Nickname;
            }
        }
    }
}
