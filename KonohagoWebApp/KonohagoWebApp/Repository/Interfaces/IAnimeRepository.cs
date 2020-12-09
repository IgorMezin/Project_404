using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonohagoWebApp.Repository.Interfaces
{
    public interface IAnimeRepository
    { 
        Task<List<Models.Anime>> GetAllAnimeAsync();
        Task AddAnime(Models.Anime anime);
        Task<List<Models.Anime>> SearchAnime(string input);
        Task<Models.Anime> GetAnimeById(int id);
    }
}
