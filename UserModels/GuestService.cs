using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class GuestService
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Service { get; set; }
        public int? IsPaid { get; set; }
        public int RoomId { get; set; }
        public int? GuestId { get; set; }
        public bool? IsNightAudited { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual Room Room { get; set; }
    }
}
