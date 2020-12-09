using System;
using System.Collections.Generic;
using System.Linq;
using KonohagoWebApp.Models;
using System.Threading.Tasks;

namespace KonohagoWebApp.Repository.Interfaces
{
    interface IComentRepository
    {
        Task<List<Comment>> GetCommentsByAnimeIdAsync(int id);
        Task AddComment(Comment comment);
    }
}
