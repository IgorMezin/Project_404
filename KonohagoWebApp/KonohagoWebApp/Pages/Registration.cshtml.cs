using KonohagoWebApp.Models;
using KonohagoWebApp.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using KonohagoWebApp.Helpers;
using System.Threading.Tasks;
namespace KonohagoWebApp.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public new User User { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
            if(HttpContext.Session.GetString("exception")== "пользователь с таким никнеймом или почтой уже существует!")
            {
                ViewData["message"] = "пользователь с таким никнеймом или почтой уже существует!";
                HttpContext.Session.SetString("exception", "");
            }   
        }
            public IActionResult OnPost()
        {
            var repository = HttpContext.RequestServices.GetService<IUserRepository>();
            bool f = repository.CheckUser(User.Email, User.Nickname);
            if (f == true)
            {
                HttpContext.Session.SetString("exception", "пользователь с таким никнеймом или почтой уже существует!");
                return Redirect("/Registration");
            }
            User.Role = Roles.User;
            repository.AddUser(User, Password);
            return Redirect("/Registration");
        }
    }
    public class AuthorizationModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var repository = HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await repository.GetUserByEmailAndPasswordAsync(Email, Password);
            HttpContext.Session.SetString("role", user.Role.ToString());
            var a = HttpContext.Session.GetString("role");
            HttpContext.Session.Set<User>("Current_user", user);
            return Redirect("/Index");
        }
    }
}
