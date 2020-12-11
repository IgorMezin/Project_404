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
        [BindProperty]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public string Email { get; set; }

        public IActionResult OnGet()
        {
            if(HttpContext.Session.GetString("role")=="User")
            {
                return Redirect("/Index");
            }
            if(HttpContext.Session.GetString("exception")!= "")
            {
                ViewData["message"] = HttpContext.Session.GetString("exception");
                HttpContext.Session.SetString("exception", "");
                return null;
            }
            return null;
        }
        public IActionResult OnPostRegistrationForm()
        {
            var repository = HttpContext.RequestServices.GetService<IUserRepository>();
            bool f = repository.CheckUser(User.Email, User.Nickname);
            if (ConfirmPassword != Password)
            {
                HttpContext.Session.SetString("exception", "пароли не совпадают");
                OnGet();
                return null;
            }
            if (f == true)
            {
                HttpContext.Session.SetString("exception", "пользователь с таким никнеймом или почтой уже существует!");
                OnGet();
                return null;
            }
            User.Role = Roles.User;
            repository.AddUser(User, Password);
            return Redirect("/Registration");
        }
        public async Task<IActionResult> OnPostAuthorizationForm()
        {
            var repository = HttpContext.RequestServices.GetService<IUserRepository>();
            bool f = repository.CheckUserPass(Email, Password);
            if (f != true)
            {
                HttpContext.Session.SetString("exception", "пользователь с таким почтой или паролем не найден!");
                return Redirect("/Registration");
            }
         
            var user = await repository.GetUserByEmailAndPasswordAsync(Email, Password);
            HttpContext.Session.SetString("role", user.Role.ToString());
            var a = HttpContext.Session.GetString("role");
            HttpContext.Session.Set<User>("Current_user", user);
            HttpContext.Session.Set("id", user.Id);
            HttpContext.Response.Cookies.Append("email", Email);
            HttpContext.Response.Cookies.Append("password", Password);
            return Redirect("/Index");
        }
    }
}
