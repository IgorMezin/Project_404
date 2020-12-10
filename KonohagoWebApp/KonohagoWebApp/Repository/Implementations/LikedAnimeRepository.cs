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

        public async Task<List<Anime>> GetLikedAnimes(int user_id)
        {
            List<Anime> likes = new List<Anime>();
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    cmd.CommandText = "select likedanime.anime_id, img_path, anime_shows.name from likedanime join anime_shows on likedanime.anime_id = anime_shows.anime_id where likedanime.user_id = @user_id";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        Anime like;
                        while (rdr.Read())
                        {
                            like = new Anime(rdr.GetInt32(rdr.GetOrdinal("anime_id")));
                            like.Name = rdr.GetString(rdr.GetOrdinal("name"));
                            if (!await rdr.IsDBNullAsync("img_path"))
                                like.img_path = rdr.GetString(rdr.GetOrdinal("img_path"));
                            else
                                like.img_path = "/img/nopreview.jpg";
                            likes.Add(like);
                        }
                    }
                }
                return likes;
            }
        }

        public bool IsLiked(Like like)
        {
            using (var connection = new NpgsqlConnection(this.connection))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@anime_id", like.Anime_id);
                    cmd.Parameters.AddWithValue("@user_id", like.User_id);
                    cmd.CommandText = "select * from likedanime where anime_id = @anime_id and user_id=@user_id";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var check = $"{rdr.GetInt32(rdr.GetOrdinal("anime_id"))}";
                            if (check != null)
                                return true;//найдено совпадение
                        
                            

                        }

                    }
                    return false;//совпадений не найдено
                }
            }
        }
    }
}
