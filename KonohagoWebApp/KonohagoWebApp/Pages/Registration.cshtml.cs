using KonohagoWebApp.Models;
using KonohagoWebApp.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace KonohagoWebApp.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public new User User { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role") != "Guest")
            {
                return Redirect("/Index");
            }

            return null;
        }

        public IActionResult OnPost()
        {
            var repository = HttpContext.RequestServices.GetService<IUserRepository>();
            User.Role = Roles.User;
            repository.AddUser(User, Password);
            return Redirect("/Index");
        }
    }
}
