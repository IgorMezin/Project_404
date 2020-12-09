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
namespace KonohagoWebApp.Pages
{
    public class TitlePageModel : PageModel
    {
        public Anime anime = new Anime();
        public async Task OnGet()
        {
            string string_id = HttpContext.Request.Query["id"];
            var rep = HttpContext.RequestServices.GetService<IAnimeRepository>();
            if (string_id == null)
                Redirect("/Index");
            else
                anime = await rep.GetAnimeById(Convert.ToInt32(string_id));
                
        }
    }
}
