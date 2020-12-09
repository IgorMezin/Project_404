using System;
using System.Data;
using System.Threading.Tasks;
using KonohagoWebApp.Models;
using KonohagoWebApp.Repository.Interfaces;
using Npgsql;
using Microsoft.AspNetCore.Http;
namespace KonohagoWebApp.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private string connection = "Host=localhost; " +
            "Username=postgres; " +
            "Password=Werrew123@; " +
            "Database=KonohagoDB";
        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                User user = new User();
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    command.CommandText = "select * from users where mail = @email and password=crypt(@password,password);";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = reader.GetInt32(reader.GetOrdinal("user_id"));
                            user.Nickname = reader.GetString(reader.GetOrdinal("nickname"));
                            user.Email = reader.GetString(reader.GetOrdinal("mail"));
                            string role = reader.GetString(reader.GetOrdinal("role"));
                            var t = reader.IsDBNullAsync("avatar");

                            if (!(await t))
                            {
                                user.ImagePath = reader.GetString(reader.GetOrdinal("avatar"));
                            }
                            else
                            {
                                user.ImagePath = "/img/noavatar.png";
                            }
                            switch (role)
                            {
                                case ("Admin"):
                                    user.Role = Roles.Admin;
                                    break;
                                case ("User"):
                                    user.Role = Roles.User;
                                    break;
                                case ("Guest"):
                                    user.Role = Roles.Guest;
                                    break;
                                default:
                                    throw new Exception("It can't be");
                            }
                        }
                    }
                }
                return user;
            }
        }
        public async Task AddUser(User user, string password)
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                await connection.OpenAsync();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into users (name,surname, nickname, password, mail, role) " +
                        "values(@name, @surname, @nickname, crypt(@password, gen_salt('bf')), @mail, @role);";
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@surname", user.Surname);
                    cmd.Parameters.AddWithValue("@nickname", user.Nickname);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@mail", user.Email);
                    cmd.Parameters.AddWithValue("@role", user.Role.ToString());
                    await cmd.ExecuteScalarAsync();
                }
            }
        }

        public bool CheckUser(string email, string nickname)
        {
            using (var connection = new NpgsqlConnection(this.connection))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@nickname", nickname);
                    cmd.CommandText = "select * from users where mail=@email or nickname=@nickname;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        string nick;
                        string mail;
                        while (reader.Read())
                        {
                            nick = reader.GetString(reader.GetOrdinal("nickname"));
                            mail = reader.GetString(reader.GetOrdinal("mail"));
                            if (nick != null || mail != null)
                            {
                                return true;
                                //найден пользователь 
                            }
                            break;
                        }
                        return false;
                        //пользователь не найден
                    }
                }
            }
        }
        public async Task<User> GetUserById(int id)
        {
            User user = new User();
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = "select * from users where user_id = @id";

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var t = rdr.IsDBNullAsync("avatar");
                            user.Nickname = rdr.GetString(rdr.GetOrdinal("nickname"));
                            user.Email = rdr.GetString(rdr.GetOrdinal("mail"));
                            user.Id = rdr.GetInt32(rdr.GetOrdinal("user_id"));
                            if(!(await t))
                            {
                                user.ImagePath = rdr.GetString(rdr.GetOrdinal("avatar"));
                            }
                            else
                            {
                                user.ImagePath = "/img/noavatar.png";
                            }
                        }
                    }
                }
            }
            return user;
        }

        public async Task UpdateUserAsync(User user, int id, string password)
        {
            await using (var connection = new NpgsqlConnection(this.connection))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@img", user.ImagePath);
                    cmd.CommandText = "update users set avatar = @img where user_id = @id";

                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }
    }
}

