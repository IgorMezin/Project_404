using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KonohagoWebApp.Helpers;
using KonohagoWebApp.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using KonohagoWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace KonohagoWebApp.Pages
{
    public class TitlePageModel : PageModel
    {
        public Anime anime = new Anime();
        public bool isLike;
        public Like like = new Like();
        public async Task<RedirectResult> OnGet()
        {
            string string_id = HttpContext.Request.Query["anime_id"];
            var rep = HttpContext.RequestServices.GetService<IAnimeRepository>();
            var rep2 = HttpContext.RequestServices.GetService<ILikedAnimeRepository>();
            if (string_id == null)
                return Redirect("/Index");
            else
            {
                anime = await rep.GetAnimeById(Convert.ToInt32(string_id));
                if (HttpContext.Session.GetString("role") == Roles.Guest.ToString())
                {
                    isLike = true;

                }
                else
                {
                    like = new Like(HttpContext.Session.Get<User>("Current_user").Id);
                    like.Anime_id = Convert.ToInt32(HttpContext.Request.Query["anime_id"]);
                    isLike = rep2.IsLiked(like);
                }
                return null;
            }
                
            
        }
        public async Task<RedirectResult> OnPost()
        {
            var rep = HttpContext.RequestServices.GetService<ILikedAnimeRepository>();
            like = new Like(HttpContext.Session.Get<User>("Current_user").Id);
            like.Anime_id = Convert.ToInt32(HttpContext.Request.Query["anime_id"]);
            await rep.AddLikeAnime(like);
            return Redirect($"/TitlePage?anime_id={Convert.ToInt32(HttpContext.Request.Query["anime_id"])}");
        }
    }
}
