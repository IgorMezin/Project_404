using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KonohagoWebApp.Models;
using KonohagoWebApp.Repository.Interfaces;
using Npgsql;
namespace KonohagoWebApp.Repository.Interfaces
{
    interface IUserRepository
    {
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task AddUser(User user, string password);
    }
}
