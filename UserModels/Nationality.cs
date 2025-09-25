using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class Nationality
    {
        public Nationality()
        {
            Guests = new HashSet<Guest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Guest> Guests { get; set; }
    }
}
