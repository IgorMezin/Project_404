using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using KonohagoWebApp.Models;
using Microsoft.Extensions.DependencyInjection;
using KonohagoWebApp.Helpers;
using KonohagoWebApp.Repository.Interfaces;
namespace KonohagoWebApp.Pages
{
    public class ProfileModel : PageModel
    {
        public IWebHostEnvironment _appEnvironment;
        [BindProperty]
        public IFormFile Avatar { get; set; }
        public User User;
        public List<Anime> animes;
        public ProfileModel(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        [BindProperty]
        public string NewNickname { get; set; }

        public async Task OnGet()
        {
            int id = Convert.ToInt32(HttpContext.Request.Query["id"]);
            var repo = HttpContext.RequestServices.GetService<IUserRepository>();
            var rep2 = HttpContext.RequestServices.GetService<ILikedAnimeRepository>();
            var user_task = repo.GetUserById(id);
            var animes_task = rep2.GetLikedAnimes(id);
            User = await user_task;
            animes = await animes_task;
        }
        private async Task<string> AddAvatar()
        {
            if (Avatar != null)
            {
                string path = "/img/" + Avatar.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await Avatar.CopyToAsync(fileStream);
                }
                return path;
            }
            else return null;
        }
        public async Task<IActionResult> OnPost()
        {
            var a = HttpContext.Session.Get<User>("Current_user");
            if(a.Id == Convert.ToInt32(HttpContext.Request.Query["id"]))
            {
                User user = new User();
                var add_image_task = AddAvatar();
                var olduser = HttpContext.Session.Get<User>("Current_user");
                var user_id = olduser.Id;
                user.ImagePath = await add_image_task;
                var task = HttpContext.RequestServices.GetService<IUserRepository>();
                HttpContext.Session.Remove("Current_user");
                await task.UpdateUserAsync(user, user_id);
                HttpContext.Session.Set("Current_user", await HttpContext.RequestServices.GetService<IUserRepository>().GetUserById(user_id));

            }
            return Redirect($"/Profile?id={HttpContext.Request.Query["id"]}");
        }
    }
}
