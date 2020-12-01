using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
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
        //public List<Models.Anime> Anime_shows = new List<Models.Anime>();
        public User User;
        public ProfileModel(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        [BindProperty]
        public string NewNickname { get; set; }

        public async Task OnGet()
        {
            if (HttpContext.Request.Query.ContainsKey("id")
                 && HttpContext.Session.GetString("role") != Roles.Guest.ToString())
            {
                int id = Convert.ToInt32(HttpContext.Request.Query["id"]);
                var repo = HttpContext.RequestServices.GetService<IUserRepository>();
                var user_task = repo.GetUserById(id);
              //var music_task = HttpContext.RequestServices.GetService<IMusicRepo>().GetMusicOfUser(id);
                User = await user_task;
              //Musics = await music_task;
            }
            else
            {
                Redirect("Index");
            }
        }
        private async Task<string> AddAvatar()
        {
            if (Avatar != null)
            {
                string path = "/imgà/" + Avatar.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await Avatar.CopyToAsync(fileStream);
                }

                return path;
            }
            else return null;
        }
    }
}
