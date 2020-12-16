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
using Microsoft.AspNetCore.Mvc.Rendering;
namespace KonohagoWebApp.Pages
{
    public class AnimeStudiosModel : PageModel
    {
        private IAnimeRepository _studio;
        public AnimeStudiosModel(IAnimeRepository studio)
        {
            _studio = studio;
        }
        public List<Studios> Studios { get; set; }
        public void OnGet()
        {

        }
        public PartialViewResult OnGetUsersPartial()
        {
            Studios = _studio.GetAllStudios();
            return Partial("_StudiosList", Studios);
        }
    }
}
