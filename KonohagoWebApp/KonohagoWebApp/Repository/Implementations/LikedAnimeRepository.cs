using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using KonohagoWebApp.Repository.Interfaces;
using KonohagoWebApp.Models;
using System.Data;
namespace KonohagoWebApp.Repository.Implementations
{
    public class LikedAnimeRepository : ILikedAnimeRepository
    {
        private string connection = "Host=localhost; " +
           "Username=postgres; " +
           "Password=Werrew123@; " +
           "Database=KonohagoDB";
        public async Task AddLikeAnime(Like like)
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@user_id", like.User_id);
                    cmd.Parameters.AddWithValue("@anime_id", like.Anime_id);
                    cmd.CommandText = "insert into likedanime(user_id, anime_id) values(@user_id, @anime_id)";
                    await cmd.ExecuteNonQueryAsync();
                }
            } 
        }

        public Task<List<Anime>> GetLikedAnimes(int user_id)
        {
            throw new NotImplementedException();
        }
    }
}
