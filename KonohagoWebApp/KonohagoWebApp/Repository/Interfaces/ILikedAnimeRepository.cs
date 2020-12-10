using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KonohagoWebApp.Models;
namespace KonohagoWebApp.Repository.Interfaces
{
    interface ILikedAnimeRepository
    {
        Task<List<Anime>> GetLikedAnimes(int user_id);
        Task AddLikeAnime(Like like);
    }
}
