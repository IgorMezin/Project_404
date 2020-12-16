using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonohagoWebApp.Models
{
    public enum Roles
    {
        Guest,
        User,
        Admin
    }
    public class User
    {
        public readonly int Id;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }

        public User(int id)
        {
            Id = id;
        }
        public User()
        {

        }

    }
}
