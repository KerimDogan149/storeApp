using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace StoreApp.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();


    }
}