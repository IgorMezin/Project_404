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
        public List<Comment> comments = new List<Comment>();
        [BindProperty]
        public Comment Comment { get; set; }

        public async Task<RedirectResult> OnGet()
        {
            
            string string_id = HttpContext.Request.Query["anime_id"];
            var rep = HttpContext.RequestServices.GetService<IComentRepository>();
            if (!HttpContext.Request.Query.ContainsKey("anime_id"))
                return Redirect("/Index");
            else
            {
                var task_comm = rep.GetCommentsByAnimeIdAsync(Convert.ToInt32(string_id));
                comments = await task_comm;
                return null;
            }
        }
        public async Task<RedirectResult> OnPost()
        {
            string text = Comment.Text;
            Comment = new Comment(HttpContext.Session.Get<User>("Current_user").Id);
            Comment.Text = text;
            var rep = HttpContext.RequestServices.GetService<IComentRepository>();
            Comment.Anime_id = Convert.ToInt32(HttpContext.Request.Query["anime_id"]);
            await rep.AddComment(Comment);
            return Redirect($"/comments?anime_id={Comment.Anime_id}");
        }
    }
}
