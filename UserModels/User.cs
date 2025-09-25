using System;
using System.Collections.Generic;

#nullable disable

namespace HotelWebApi.UserModels
{
    public partial class User
    {
        public User()
        {
            UserApplications = new HashSet<UserApplication>();
        }

        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public string? Gender { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
