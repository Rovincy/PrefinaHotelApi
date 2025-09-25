using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class Room
    {
        public Room()
        {
            Billings = new HashSet<Billing>();
            BillingsHistories = new HashSet<BillingsHistory>();
            Bookings = new HashSet<Booking>();
            GuestServices = new HashSet<GuestService>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public int? TypeId { get; set; }
        public bool? IsActive { get; set; }

        public virtual RoomType Type { get; set; }
        public virtual ICollection<Billing> Billings { get; set; }
        public virtual ICollection<BillingsHistory> BillingsHistories { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<GuestService> GuestServices { get; set; }
    }
}
