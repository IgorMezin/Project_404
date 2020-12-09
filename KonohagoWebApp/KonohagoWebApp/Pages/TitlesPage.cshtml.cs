using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KonohagoWebApp.Models;
using Microsoft.Extensions.DependencyInjection;
using KonohagoWebApp.Repository.Interfaces;
namespace KonohagoWebApp.Pages
{
    public class TitlesPageModel : PageModel
    {
        public List<Anime> animes = new List<Anime>();
        public async Task OnGet()
        {
            string searchText = HttpContext.Request.Query["search_input"];
            var rep = HttpContext.RequestServices.GetService<IAnimeRepository>();
            if(searchText == null)
            {
                var task = rep.GetAllAnimeAsync();
                animes = await task;
            }
            else
            {
                animes = await rep.SearchAnime(searchText);
            }
        }
    }
}
