using System;
using System.Collections.Generic;

#nullable disable

namespace HotelWebApi.UserModels
{
    public partial class Application
    {
        public Application()
        {
            UserApplications = new HashSet<UserApplication>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
