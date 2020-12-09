using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KonohagoWebApp.Models;
using KonohagoWebApp.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using KonohagoWebApp.Helpers;
namespace KonohagoWebApp.Pages
{
    public class commentsModel : PageModel
    {
        List<Comment> comments = new List<Comment>();
        [BindProperty]
        public Comment Comment { get; set; }
        public async Task OnGet()
        {
            string string_id = HttpContext.Request.Query["anime_id"];
            var rep = HttpContext.RequestServices.GetService<IComentRepository>();
            if (string_id == null)
                Redirect("/Index");
            else
            {
                var task_comm = rep.GetCommentsByAnimeIdAsync(Convert.ToInt32(string_id));
                comments = await task_comm;
            }
        }
        public async Task<RedirectResult> OnPost()
        {
            Comment = new Comment(HttpContext.Session.Get<User>("Current_user").Id);
            var rep = HttpContext.RequestServices.GetService<IComentRepository>();
            Comment.Anime_id = Convert.ToInt32(HttpContext.Request.Query["anime_id"]);
            await rep.AddComment(Comment);
            return Redirect($"/comments?anime_id={Comment.Anime_id}");
        }
    }
}
