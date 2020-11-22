using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KonohagoWebApp.Models;
using KonohagoWebApp.Repository.Interfaces;
using Npgsql;
using NpgsqlTypes;
namespace KonohagoWebApp.Repository.Implementations
{ 
    public class UserRepository : IUserRepository
    {
        private string connection = "Host=localhost; " +
            "Username=postgres; " +
            "Password=Werrew123@; " +
            "Database=Konohago";
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
                    command.CommandText = "select * from users" +
                                         "where mail = @email and password=crypt(@password,password);";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Nickname = reader.GetString(reader.GetOrdinal("nickname"));
                            user.Email = reader.GetString(reader.GetOrdinal("mail"));
                            string role = reader.GetString(reader.GetOrdinal("role"));
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
            using (var connection = new NpgsqlConnection(this.connection))
            {                
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into users (name,surname, nickname, mail, password, role)" +
                                          "values(@name, @surname, @nickname, @mail, crypt(@password, gen_salt('bf')), @role);";
                    command.Parameters.AddWithValue("email",NpgsqlDbType.Varchar, user.Email);
                    command.Parameters.AddWithValue("password", NpgsqlDbType.Varchar, password);
                    command.Parameters.AddWithValue("nickname", NpgsqlDbType.Varchar, user.Nickname);
                    command.Parameters.AddWithValue("name", NpgsqlDbType.Varchar, user.Name);
                    command.Parameters.AddWithValue("surname", NpgsqlDbType.Varchar, user.Surname);
                    command.Parameters.AddWithValue("role", NpgsqlDbType.Varchar, user.Role.ToString());
                    await command.ExecuteScalarAsync();

                }
            }
        }
    }

}

