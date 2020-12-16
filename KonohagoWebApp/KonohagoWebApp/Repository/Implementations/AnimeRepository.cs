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
    public class AnimeRepository : IAnimeRepository
    {
        private string connection = "Host=localhost; " +
           "Username=postgres; " +
           "Password=Werrew123@; " +
           "Database=KonohagoDB";

        public async Task AddAnime(Anime anime)
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                await connection.OpenAsync();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into anime_shows (img_path, year, name, description) " +
                        "values(@img_path, @year, @name, @desc);";
                    cmd.Parameters.AddWithValue("@name", anime.Name);
                    cmd.Parameters.AddWithValue("@img_path", anime.img_path);
                    cmd.Parameters.AddWithValue("@year" , anime.year);
                    cmd.Parameters.AddWithValue("@desc", anime.Desc);
                    await cmd.ExecuteScalarAsync();
                }
            }
        }

        public async Task<List<Anime>> GetAllAnimeAsync()
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                List<Anime> animes = new List<Anime>();
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select anime_id, name, img_path from anime_shows";
                    using (var rdr = cmd.ExecuteReader())
                    { 
                        while (rdr.Read())
                        {
                            Anime anime = new Anime(rdr.GetInt32(rdr.GetOrdinal("anime_id")));
                            if (!rdr.IsDBNull("img_path"))
                            {
                                anime.img_path = rdr.GetString(rdr.GetOrdinal("img_path"));
                            }
                            else 
                            {
                                anime.img_path = "/img/nopreview.jpg";
                            }
                            anime.Name = rdr.GetString(rdr.GetOrdinal("name"));
                            animes.Add(anime);
                        }
                    }
                }
                return animes;
            }
            
        }

        public List<Studios> GetAllStudios()
        {
            using (var connection = new NpgsqlConnection(this.connection))
            {
                List<Studios> studios = new List<Studios>();
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT studio_id, name, foundation_date FROM studios";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Studios studio = new Studios(rdr.GetInt32(rdr.GetOrdinal("studio_id")));
                            studio.Name = rdr.GetString(rdr.GetOrdinal("name"));
                            studio.Foundation_date = rdr.GetDateTime(rdr.GetOrdinal("foundation_date"));
                            studios.Add(studio);
                        }
                    }
                    return studios;
                }
            }
        }

        public async Task<Anime> GetAnimeById(int id)
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                Anime anime = new Anime();
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = "select * from anime_shows where anime_id = @id";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            anime = new Anime(rdr.GetInt32(rdr.GetOrdinal("anime_id")));
                            if (!rdr.IsDBNull("img_path"))
                            {
                                anime.img_path = rdr.GetString(rdr.GetOrdinal("img_path"));
                            }
                            else
                            {
                                anime.img_path = "/img/nopreview.jpg";
                            }
                            anime.Name = rdr.GetString(rdr.GetOrdinal("name"));
                            anime.Desc = rdr.GetString(rdr.GetOrdinal("description"));
                        }
                    }
                }
                return anime;
            }
        }

        public async Task<List<Anime>> SearchAnime(string input)
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                List<Anime> animes = new List<Anime>();
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    input = "%"+input+"%";
                    cmd.Parameters.AddWithValue("@input", input);
                    cmd.CommandText = "select * from anime_shows where lower(name) like lower(@input)";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Anime anime = new Anime(rdr.GetInt32(rdr.GetOrdinal("anime_id")));
                            if (!rdr.IsDBNull("img_path"))
                            {
                                anime.img_path = rdr.GetString(rdr.GetOrdinal("img_path"));
                            }
                            else
                            {
                                anime.img_path = "/img/nopreview.jpg";
                            }
                            anime.Name = rdr.GetString(rdr.GetOrdinal("name"));
                            animes.Add(anime);
                        }
                    }
                    return animes;
                }
            }
        }
    }
}
